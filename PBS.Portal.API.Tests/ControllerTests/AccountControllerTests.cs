using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Common;
using PBS.Portal.API.Controllers;
using PBS.Portal.API.Data.CommandHandlers;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Data.QueriesHandlers;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;
using PBS.Portal.API.Tests.QueryHandlerTests;

namespace PBS.Portal.API.Tests.ControllerTests
{
    [TestFixture]
    class AccountControllerTests
    {
        private AccountController _controller;
        private Mock<ICommandDispatcher> _mockCommandDispatcher;
        private Mock<IQueryDispatcher> _mockQueryDispatcher;

        private static readonly Guid ExistingGuid = Guid.Parse("11111111-2222-3333-4444-555555555555");
        private static readonly Guid NonExistingGuid = Guid.Parse("00000000-2222-3333-4444-555555555555");

        private RegisterViewModel _emptyRegisterViewModel;
        private RegisterViewModel _existingRegisterViewModel;
        private RegisterViewModel _registerViewModel; // delete

        [SetUp]
        public void Setup()
        {
            _registerViewModel = new RegisterViewModel();
            _emptyRegisterViewModel = new RegisterViewModel();
            _existingRegisterViewModel = new RegisterViewModel { PendingUserId = ExistingGuid };

            _mockCommandDispatcher = new Mock<ICommandDispatcher>();
            _mockCommandDispatcher
                .Setup(m => m.Dispatch(new RegisterCommand()))
                .Verifiable();

            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockQueryDispatcher
                .Setup(m => m.Dispatch(It.IsAny<GetPendingUserDetailsByGuidQuery>()))
                .Returns((GetPendingUserDetailsByGuidQuery query) => TestData.PendingUsersList.First(p => p.Id == query.PendingUserId).ToRegisterViewModel());


            //_getGetUserProfileByUserNameQuery = new GetUserProfileByUserNameQuery {UserName = "w.pooh@honeypot.com"};
            //_mockQueryDispatcher
            //    .Setup(m => m.Dispatch(_getGetUserProfileByUserNameQuery))
            //    .Returns(new UserViewModel() { UserName = "w.pooh@honeypot.com" } );

            //_mockQueryDispatcher
            //    .Setup(m => m.Dispatch(new GetNewUserDetailsByGuidQuery() { Id = ExistingGuid }))
            //    .Returns(TestData.PendingUsersList);

            _controller = new AccountController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);
        }


        [Test]
        public void GetPendingUsers_Returns_User_With_Same_Guid()
        {
            //http://www.asp.net/web-api/overview/testing-and-debugging/unit-testing-controllers-in-web-api
            // Arrange done in Setup()

            // Act
            var returnedUser = _controller.GetPendingUser(ExistingGuid);
            var contentResult = returnedUser as OkNegotiatedContentResult<RegisterViewModel>;

            // Assert
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(ExistingGuid, contentResult.Content.PendingUserId);
        }

        [Test]
        public void DeleteUser_Makes_Call_To_Correct_Handler()
        {
            var deletePendingUserByGuidCommand = new DeletePendingUserByGuidCommand { PendingUserId = ExistingGuid };
            //For<ICommandHandler<DeletePendingUserByGuidCommand>>().Use<DeletePendingUserByGuidCommandHandler>();
            //var actionResult = _controller.DeletePendingUser(ExistingGuid);

        }

        [Test]
        public void AssemblyName()
        {
            var name = MethodBase.GetCurrentMethod().DeclaringType.Namespace;
            //Assert.AreEqual("", name);
        }

        //[Test]
        public void Given_PendingUserId_Doesnt_Exist_GetPendingUsers_Returns_Empty_RegisterViewModel()
        {
            var returnedUser = _controller.GetPendingUser(Guid.Empty);

            Assert.IsNotNull((returnedUser));
            //Assert.AreEqual(Guid.Empty, returnedUser.PendingUserId);
        }

        //[Test]
        public void AccountController_Register_Method_Always_Returns_A_Response()
        {
            var httpActionResult = _controller.Register(_emptyRegisterViewModel);
            var response = httpActionResult as OkNegotiatedContentResult<string>;
            Debug.Assert(response != null);
            Assert.IsNotNullOrEmpty(response.Content);

            httpActionResult = _controller.Register(_existingRegisterViewModel);
            response = httpActionResult as OkNegotiatedContentResult<string>;
            Debug.Assert(response != null);
            Assert.AreEqual("", response.Content);
        }

        //[Test]
        public void Account_GetPendingUsers_Returns_A_List()
        {
            var newUserDetails = _controller.GetPendingUser(ExistingGuid);
            Assert.IsNotNull(newUserDetails);
        }

        //[Test]
        public void Test_Register_Returns_Valid_IHttpActionResult_To_Indicate_Method_Was_Called()
        {
            _registerViewModel = new RegisterViewModel() { PendingUserId = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb8"), Password = "AppleApple1!", ConfirmPassword = "AppleApple1!" };
            var httpActionResult = _controller.Register(_registerViewModel);
            Assert.IsInstanceOf<OkNegotiatedContentResult<string>>(httpActionResult);

            var response = httpActionResult as OkNegotiatedContentResult<string>;
            Debug.Assert(response != null);
            Assert.IsNotNullOrEmpty(response.Content);
        }

        //[Test]
        public void Given_RegisterViewModel_Is_Valid_And_Not_Registered_Register_Method_Returns_Registration_Successful()
        {
            _registerViewModel = new RegisterViewModel() { PendingUserId = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb8"), Password = "AppleApple1!", ConfirmPassword = "AppleApple1!" };

            var httpActionResult = _controller.Register(_registerViewModel);
            var response = httpActionResult as OkNegotiatedContentResult<string>;
            Debug.Assert(response != null);
            Assert.AreEqual("Registration Successful", response.Content);
        }

        //[Test]
        public void Given_Guid_Return_User_Details_From_Staging_Table()
        {
            var existingUserGuid = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb1");
            var getNewUserDetailsByGuid = new GetPendingUserDetailsByGuidQuery();
            Assert.IsNotNull(getNewUserDetailsByGuid);
            Assert.AreEqual(Guid.NewGuid(), getNewUserDetailsByGuid.PendingUserId);
        }


        [Test]
        public void Given_CAN_When_Query_Is_Valid_then_return_ClientEntryPointURL()
        {
            string dummyUrl = "http://dummyurl";
            
            _mockQueryDispatcher
                .Setup(m => m.Dispatch<EntryPointURLModel>(It.IsAny<IQuery<EntryPointURLModel>>()))
                .Returns(new EntryPointURLModel(dummyUrl));
                     
            AccountController controller = new AccountController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);

            GetClientEntryPointQuery qry = new GetClientEntryPointQuery();
            qry.CAN = "DEMO002";
            IHttpActionResult actionResult = controller.GetEntryPointForClient( qry );

            Assert.IsInstanceOf<OkNegotiatedContentResult<EntryPointURLModel>>(actionResult);

            var contentResult = actionResult as OkNegotiatedContentResult<EntryPointURLModel>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            Assert.AreEqual( dummyUrl, contentResult.Content.URL );
        }


        public static class TestData
        {
            public static readonly List<PendingUser> PendingUsersList = new List<PendingUser>
            {
                new PendingUser { Id = ExistingGuid, Email = "hubbabubba@bubble.gum", CAN = "CANCAN", FirstName = "Winnie", LastName = "Pooh" },
                new PendingUser { Id = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb9"), Email = "wrigleysspearming@chewing.gum", CAN = "CANCAN1", FirstName = "Tigger", LastName = "Tiglon" },
            };
        }
    }
}
