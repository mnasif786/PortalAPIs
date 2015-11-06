using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.CommandHandlers;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Controllers
{
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        // I place these up here to enable us to export strings to another class in future.
        private const string PendingUserNotFound = "PendingUser for Id:{0} could not be retrieved from PendingUsers";
        private const string UserAlreadyExists = "User with Email:{0} already exists in the Portal Database";
        private const string PeninsulaClientNotFound = "ClientId for CAN:{0} could not be retrieved from PeninsulaClients";
        private const string Success = "Registration Successful";
        private const string Failed = "Failed to register a new user with PendingUserId: {0}";

        public AccountController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [Route("Pending/{pendingUserId}")]
        public IHttpActionResult GetPendingUser(Guid pendingUserId)
        {
            var pendingUserQuery = new GetPendingUserDetailsByGuidQuery { PendingUserId = pendingUserId };
            var result = _queryDispatcher.Dispatch(pendingUserQuery);

            if (result != null)
            {
                return Ok(result);
            }
            
            return ResponseMessage(
                Request.CreateErrorResponse(
                    HttpStatusCode.PreconditionFailed,
                    string.Format(PendingUserNotFound, pendingUserId)
                )
            );
        }

        public void DeletePendingUser(Guid pendingUserId)
        {
            var deletePendingUserByGuid = new DeletePendingUserByGuidCommand { PendingUserId = pendingUserId };
            _commandDispatcher.Dispatch(deletePendingUserByGuid);
        }

        // @todo can we pass in a RegisterCommand rather than a RegisterViewModel?
        // The initial thought was to use a ViewModel so i could take advantage of ModelState.IsValid
        // This was when we were posting in the FirstName, LastName, Email, and Passwords. 
        [Route("Register")]
        [HttpPost]
        public IHttpActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errorList = ModelState.ToDictionary(kvp => kvp.Key.Replace("model.", ""), kvp => kvp.Value.Errors[0].ErrorMessage);
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, errorList));
            }

            var pendingUser = _queryDispatcher.Dispatch(new GetPendingUserDetailsByGuidQuery { PendingUserId = model.PendingUserId });
            if (pendingUser == null)
            {
                return ResponseMessage(
                    Request.CreateErrorResponse(
                        HttpStatusCode.PreconditionFailed, 
                        string.Format(PendingUserNotFound, model.PendingUserId)
                    )
                );
            }

            var peninsulaClient = _queryDispatcher.Dispatch(new GetPeninsulaClientIdByCanQuery { CAN = pendingUser.CAN });
            if (peninsulaClient == null)
            {
                return ResponseMessage(
                    Request.CreateErrorResponse(
                        HttpStatusCode.ExpectationFailed,
                        string.Format(PeninsulaClientNotFound, pendingUser.CAN)
                    )
                );
            }

            var existingUser = _queryDispatcher.Dispatch(new GetUserProfileByUserNameQuery { UserName = pendingUser.Email });
            if (existingUser != null)
            {
                return ResponseMessage(
                    Request.CreateErrorResponse(
                        HttpStatusCode.Conflict, 
                        string.Format(UserAlreadyExists, pendingUser.Email)
                    )
                );
            }

            try
            {
                var registerCommand = new RegisterCommand
                {
                    Email = pendingUser.Email,
                    Password = model.Password,
                    ConfirmPassword = model.ConfirmPassword,
                    FirstName = pendingUser.FirstName,
                    LastName = pendingUser.LastName,
                    ClientId = peninsulaClient.ClientId
                };

                var sendRegistrationCompleteEmailCommand = new SendRegistrationCompleteEmailCommand
                {
                    Email = pendingUser.Email
                };

                _commandDispatcher.Dispatch(registerCommand);
                _commandDispatcher.Dispatch(sendRegistrationCompleteEmailCommand);

                DeletePendingUser(pendingUser.PendingUserId);
                return Ok(Success);
            }
            catch (Exception ex)
            {
                throw new ArgumentException(string.Format(Failed, model.PendingUserId));
            }
            #region attempted to hook into the default ApplicationUser to check if a user exists and enforce the password strength. will need some help with that
            //var user = new ApplicationUser { Email = model.Email, UserName = model.Email };
            //var result = ApplicationUserManager.Create(user, model.Password);
            //if (result.Succeeded)
            //{
            //    var ctx = this.Request.GetOwinContext();
            //    UserManager.AddClaim(user.Id, new Claim("PBS.ClientId", model.CAN));
            //    response = "Registration Successful";
            //}
            //else
            //{
            //    response = result.Errors;
            //}
            #endregion
        }


        [Route("EntryPointURL")]
        [HttpGet]
        public IHttpActionResult GetEntryPointForClient([FromUri] GetClientEntryPointQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

            var result = _queryDispatcher.Dispatch(query);

            return Ok(result);
        }
        
        [Route("ChangePassword")]
        [HttpPost]
        public IHttpActionResult ChangePassword(ChangePasswordCommand changePasswordCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resposne = _commandDispatcher.Dispatch<ChangePasswordCommand, CommandResultViewModel>(changePasswordCommand);

            if (!resposne.CommandExecuted)
            {
                ModelState.AddModelError("Invalid Request",resposne.Message);
                return BadRequest(ModelState);
            }
           
            return Ok();
        }

        [HttpPost]
        [Route("SendPasswordResetEmail")]
        public IHttpActionResult SendPasswordResetEmail(SendPasswordResetEmailCommand sendPasswordResetEmailCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var resposne = _commandDispatcher.Dispatch<SendPasswordResetEmailCommand, CommandResultViewModel>(sendPasswordResetEmailCommand);

            if (!resposne.CommandExecuted)
            {
                ModelState.AddModelError("Invalid Request", resposne.Message);
                return BadRequest(ModelState);
            }

            return Ok(); 
        }

        [Route("Portal/User/{UserId}")]
        public IHttpActionResult GetPortalUserById([FromUri]GetPortalUserByIdQuery getPortalUserByIdQuery)
        {
            if (getPortalUserByIdQuery == null)
            {
                throw new ArgumentNullException("getPortalUserByIdQuery");
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //Decodes the userId
            getPortalUserByIdQuery.UserId = Helpers.Base64ForUrlDecode(getPortalUserByIdQuery.UserId);
            var result = _queryDispatcher.Dispatch(getPortalUserByIdQuery);

            return Ok(result);
        }

        [Route("ResetPassword")]
        [HttpPost]
        public IHttpActionResult ResetPassword(ResetPasswordCommand resetPasswordCommand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var resposne = _commandDispatcher.Dispatch<ResetPasswordCommand, CommandResultViewModel>(resetPasswordCommand);

            if (!resposne.CommandExecuted)
            {
                ModelState.AddModelError("Invalid Request", resposne.Message);
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}
