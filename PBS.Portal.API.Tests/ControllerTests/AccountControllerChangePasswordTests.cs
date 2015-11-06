using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Controllers;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Tests.ControllerTests
{
    [TestFixture]
    public class AccountControllerChangePasswordTests
    {
        private Mock<ICommandDispatcher> _mockCommandDispatcher;
        private Mock<IQueryDispatcher> _mockQueryDispatcher;

        [TestFixtureSetUp]
        public void Setup()
        {
            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockCommandDispatcher = new Mock<ICommandDispatcher>();
        }

        [Test]
        public void Given_change_password_request_when_ChangePasswordCommand_is_invalid_then_return_InvalidModelStateResult()
        {
            var inValidChangePasswordCommand = new ChangePasswordCommand {};

            var controller = GetTarget();
            controller.ModelState.AddModelError("UserId", "UserId is required."); //Mocks the model state
            var response = controller.ChangePassword(inValidChangePasswordCommand);
            
            Assert.IsInstanceOf<InvalidModelStateResult>(response);
        }

        [Test]
        public void Given_change_password_request_when_ChangePasswordCommand_is_valid_then_ChangePasswordCommand_is_dispatched_sucessfully()
        {
            ChangePasswordCommand dispatchedChangePasswordCommand = null;

            var validChangePasswordCommand = new ChangePasswordCommand { UserId = "1" };
            var commandResultViewModel = new CommandResultViewModel { CommandExecuted = true};

            _mockCommandDispatcher.Setup(c => c.Dispatch<ChangePasswordCommand, CommandResultViewModel>(validChangePasswordCommand))
                .Returns(commandResultViewModel)
                .Callback<ChangePasswordCommand>(x => dispatchedChangePasswordCommand = x);

            var controller = GetTarget();
            var response = controller.ChangePassword(validChangePasswordCommand);

            Assert.AreEqual(validChangePasswordCommand.UserId, dispatchedChangePasswordCommand.UserId);
           
        }

        [Test]
        public void Given_change_password_request_when_ChangePasswordCommand_is_valid_then_return_OkResult()
        {
            var validChangePasswordCommand = new ChangePasswordCommand { UserId = "1"};

            var commandResultViewModel = new CommandResultViewModel { CommandExecuted = true};
            _mockCommandDispatcher.Setup(
                c => c.Dispatch<ChangePasswordCommand, CommandResultViewModel>(validChangePasswordCommand))
                .Returns(commandResultViewModel);
            
            var controller = GetTarget();
            var response = controller.ChangePassword(validChangePasswordCommand);
            
            Assert.IsInstanceOf<OkResult>(response);
        }

        private AccountController GetTarget()
        {
            return new AccountController(_mockCommandDispatcher.Object, _mockQueryDispatcher.Object);
        }
    }
}
