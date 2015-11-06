using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using PBS.Core.CQRS.Query;
using PBS.PortalAdmin.API.Controllers;
using PBS.PortalAdmin.API.Data.Queries;
using PBS.PortalAdmin.API.Models;
using PBS.PortalAdmin.API.Tests.Helpers;

namespace PBS.PortalAdmin.API.Tests.ControllerTests
{
    [TestFixture]
    public class AdminControllerTests
    {
        private Mock<IQueryDispatcher> _mockQueryDispatcher;
        private GetPortalUsersQuery _getPortalUsersQuery;
        

        private IList<PortalUserViewModel> _portalUserViewModelList()
        {
            var userViewModelList = new List<PortalUserViewModel>
            {
                new PortalUserViewModel{ClientId = 1, UserId = "1", Email = "na@na.com"}
            };

            return userViewModelList;
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            _getPortalUsersQuery = new GetPortalUsersQuery { CanOrCompanyName = "Demo1" };
            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockQueryDispatcher.Setup(q => q.Dispatch(_getPortalUsersQuery)).Returns(_portalUserViewModelList);
        }

        [Test]
        public void Given_Ad_User_is_authenticated_when_GetLoggedInUser_is_called_then_return_user_with_domain_name()
        {
            var userDomainName = "hq\\nauman.asif";
            //Get Mocked Ad User
            Thread.CurrentPrincipal = AdUser.Create(userDomainName, true);
            
            var controller = GetTarget();
            var actionResult = controller.GetLoggedInUser();

            Assert.IsInstanceOf<OkNegotiatedContentResult<AdUserViewModel>>(actionResult);
            var contentResult = actionResult as OkNegotiatedContentResult<AdUserViewModel>;

            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.That(contentResult.Content.DomainName, Is.EqualTo(userDomainName));
        }


        [Test]
        public void Given_CanOrCompanyName_When_GetPortalUsers_is_called_then_return_users()
        {
            
            var controller = GetTarget();
            var actionResult = controller.GetPortalUsers(_getPortalUsersQuery);
            Assert.IsInstanceOf<OkNegotiatedContentResult<IList<PortalUserViewModel>>>(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<IList<PortalUserViewModel>>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.That(contentResult.Content.Count, Is.EqualTo(1));
        }

        [Test]
        public void Given_CanOrCompanyName_When_GetPortalUsers_is_called_and_no_users_found_then_return_Not_Found()
        {
            //mocked getPortalUsersByCanQuery and queryDispatcher
            var getPortalUsersQuery = new GetPortalUsersQuery { CanOrCompanyName = "Demo2" }; 
            var qryDispatcher = new Mock<IQueryDispatcher>();
            qryDispatcher.Setup(q => q.Dispatch(getPortalUsersQuery)).Returns((IList<PortalUserViewModel>)null);

            var controller = GetTarget();
            var actionResult = controller.GetPortalUsers(getPortalUsersQuery);
            Assert.That(actionResult, Is.InstanceOf<NotFoundResult>());
        }


        [Test]
        public void Given_VerifyClientRegistrationQuery_With_CliendIdOrCan_Value_Demo1_When_VerifyClientRegistration_is_called_then_return_true()
        {

            var clientRegisterationVerificationQuery = new ClientRegisterationVerificationQuery { ClientIdOrCan = "Demo1" };
            _mockQueryDispatcher.Setup(q => q.Dispatch<bool>(clientRegisterationVerificationQuery)).Returns(true);

            var controller = GetTarget();
            var actionResult = controller.VerifyClientRegistration(clientRegisterationVerificationQuery);
            Assert.IsInstanceOf<OkNegotiatedContentResult<bool>>(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<bool>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.That(contentResult.Content, Is.True);
        }

        [Test]
        public void Given_VerifyClientRegistrationQuery_With_CliendIdOrCan_Value_Demo5_When_VerifyClientRegistration_is_called_then_return_false()
        {

            var clientRegisterationVerificationQuery = new ClientRegisterationVerificationQuery { ClientIdOrCan = "Demo5" };
            _mockQueryDispatcher.Setup(q => q.Dispatch<bool>(clientRegisterationVerificationQuery)).Returns(false);

            var controller = GetTarget();
            var actionResult = controller.VerifyClientRegistration(clientRegisterationVerificationQuery);
            Assert.IsInstanceOf<OkNegotiatedContentResult<bool>>(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<bool>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.That(contentResult.Content, Is.False);
        }
        
        public AdminController GetTarget()
        {
            return new AdminController(_mockQueryDispatcher.Object);
        }
    }


}
