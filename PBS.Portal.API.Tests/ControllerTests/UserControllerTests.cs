using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using PBS.Core.CQRS.Command;
using PBS.Core.CQRS.Query;
using PBS.Portal.API.Controllers;
using PBS.Portal.API.Data.Commands;

namespace PBS.Portal.API.Tests.ControllerTests
{
     [TestFixture]
    public class UserControllerTests
    {
        private Mock<IQueryDispatcher> _mockQueryDispatcher;
        private Mock<ICommandDispatcher> _mockCommandDispatcher;       

        [TestFixtureSetUp]
        public void SetUp()
        {            
            _mockQueryDispatcher = new Mock<IQueryDispatcher>();
            _mockCommandDispatcher = new Mock<ICommandDispatcher>();           
        }


        [Test]
        public void Given_Portal_User_Exists_when_Send_Registration_Email_is_called_then_registration_link_is_sent_in_email()
        {
            SendRegistrationEmailCommand dispatchedCmd = null;
            _mockCommandDispatcher
                .Setup(q => q.Dispatch(It.IsAny<SendRegistrationEmailCommand>()))
                .Callback<SendRegistrationEmailCommand>(c => dispatchedCmd = c);

            var controller = GetTarget();

            SendRegistrationEmailCommand cmd = new SendRegistrationEmailCommand() { Email = "barney.mcgrew@trumpton.co.uk" };
            var actionResult = controller.SendRegistrationEMail(cmd);

            Assert.IsInstanceOf<OkResult>(actionResult);

            Assert.AreEqual(cmd.Email, dispatchedCmd.Email);
        }

        public UserController GetTarget()
        {
            return new UserController(_mockQueryDispatcher.Object, _mockCommandDispatcher.Object );
        }
    }
}



