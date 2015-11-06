using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.CommandHandlers;
using PBS.Portal.API.Data.Commands;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Extensions;
using PBS.Portal.API.Models;


namespace PBS.Portal.API.Tests.CommandHandlerTests
{
    [TestFixture]
    public class ChangePasswordCommandHandlerTest
    {
        private Mock<IApplicationDbContext> _mockDbContext;
        private Mock<DbSet<AspNetUser>> _mockUserMock;
        private Mock<IEmailExtenionsWrapper> _mockEmailExtensionWrapper;
       
        [SetUp]
        public void TestSetup()
        {
            _mockUserMock = new Mock<DbSet<AspNetUser>>();
            
            var userData = GetUsers().AsQueryable();

            _mockUserMock.As<IQueryable<AspNetUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            _mockUserMock.As<IQueryable<AspNetUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            _mockUserMock.As<IQueryable<AspNetUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            _mockUserMock.As<IQueryable<AspNetUser>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator);

            // DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext.Setup(m => m.Users).Returns(_mockUserMock.Object);

            _mockEmailExtensionWrapper = new Mock<IEmailExtenionsWrapper>();
            EmailExtensions.EmailExtensionWrapper = _mockEmailExtensionWrapper.Object;
           

        }

        
        [Test]
        public void Given_registered_user_make_password_change_request_when_user_Id_not_found_then_command_execution_fails_with_message_user_not_found() 
        {
            var command = new ChangePasswordCommand { UserId = "2"};
            var commandHandler = new ChangePasswordCommandHandler(_mockDbContext.Object);
            
            var result  =  commandHandler.Execute(command);
            Assert.That(result.CommandExecuted, Is.EqualTo(false));
            Assert.That(result.Message, Is.EqualTo("User Id Not Found"));
        }

        [Test]
        public void Given_registered_user_make_password_change_request_when_User_Name_is_incorrect_then_command_execution_fails_with_message_User_credentials_not_verified()
        {
            var command = new ChangePasswordCommand { UserId = "1", UserName = "na@na.com", OldPassword = "999" };
            var commandHandler = new ChangePasswordCommandHandler(_mockDbContext.Object);

            var result =commandHandler.Execute(command);
            Assert.That(result.CommandExecuted, Is.EqualTo(false));
            Assert.That(result.Message, Is.EqualTo("User credentials not verified"));
        }

        [Test]
        public void Given_registered_user_make_password_change_request_when_Old_Password_does_not_match_then_command_execution_fails_with_message_User_credentials_not_verified()
        {
            var command = new ChangePasswordCommand { UserId = "1", UserName = "nauman@abc.com", OldPassword = "999" };
            var commandHandler = new ChangePasswordCommandHandler(_mockDbContext.Object);

            var result = commandHandler.Execute(command);
            Assert.That(result.CommandExecuted, Is.EqualTo(false));
            Assert.That(result.Message, Is.EqualTo("User credentials not verified"));
        }


        [Test]
        public void Given_registered_user_make_password_change_request_when_User_credientials_are_verified_then_old_password_change()
        {
            _mockDbContext.Setup(x => x.Save()).Returns(1); //You need to mock db context save changes return value 
            _mockEmailExtensionWrapper.Setup(m => m.SendPasswordChangedEmail(It.IsAny<AspNetUser>()));

            //credentials i.e. UserName and OldPassword
            var command = new ChangePasswordCommand { UserId = "1", UserName = "nauman@abc.com", OldPassword = "123", NewPassword = "456", ConfirmNewPassword = "456" };
            var commandHandler = new ChangePasswordCommandHandler(_mockDbContext.Object);

            //Password before executing command
            var oldPassword = _mockUserMock.Object.ToList()[0].PasswordHash;
            
           var result = commandHandler.Execute(command);

            Assert.That(result.CommandExecuted, Is.EqualTo(true));
            Assert.That(_mockUserMock.Object.ToList()[0].PasswordHash, !Is.EqualTo(oldPassword));
            
        }


        private static IEnumerable<AspNetUser> GetUsers()
        {
            var users = new List<AspNetUser>
           {
             new AspNetUser{ Id = "1", Email = "nauman@abc.com", PasswordHash = new PasswordHasher().HashPassword("123"), UserName = "nauman@abc.com", 
                 UserProfile = new UserProfile{ FirstName = "Nauman", LastName = "Asif"} },
            
           };

           return users;
        }
    }

    
}
