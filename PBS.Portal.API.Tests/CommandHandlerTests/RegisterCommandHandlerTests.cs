using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using PBS.Portal.API.Common;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Data.CommandHandlers;
using PBS.Portal.API.Data.Commands;

namespace PBS.Portal.API.Tests.CommandHandlerTests
{
    [TestFixture]
    public class RegisterCommandHandlerTests
    {
        private Mock<IApplicationDbContext> _mockDbContext;

        [SetUp]
        public void Setup()
        {
            var mockAspNetUsers = new Mock<DbSet<AspNetUser>>();

            var aspNetUsersData = TestData.AspNetUsersList.AsQueryable();

            mockAspNetUsers.As<IQueryable<AspNetUser>>()
                .Setup(m => m.Provider)
                .Returns(aspNetUsersData.Provider);

            mockAspNetUsers.As<IQueryable<AspNetUser>>()
                .Setup(m => m.Expression)
                .Returns(aspNetUsersData.Expression);

            mockAspNetUsers.As<IQueryable<AspNetUser>>()
                .Setup(m => m.ElementType)
                .Returns(aspNetUsersData.ElementType);

            mockAspNetUsers.As<IQueryable<AspNetUser>>()
                .Setup(m => m.GetEnumerator())
                .Returns(aspNetUsersData.GetEnumerator);

            


            _mockDbContext = new Mock<IApplicationDbContext>();

            _mockDbContext
                .Setup(m => m.Users)
                .Returns(mockAspNetUsers.Object);
        }

        [Test]
        public void TestingExecute()
        {
            var handler = new RegisterCommandHandler(_mockDbContext.Object);
            
            var command = new RegisterCommand() { Email = "LH@peninsula-uk.com", Password = "444", ConfirmPassword = "444"};
            var aspNetUser = new AspNetUser() { Id = "4", Email = command.Email, PasswordHash = command.Password};

            var currentNumberOfUsers = TestData.AspNetUsersList.Count;

            _mockDbContext.Object.Users.Add(aspNetUser);
            

            //Assert.IsTrue(TestData.AspNetUsersList.Count > currentNumberOfUsers);

        }
    }


    public static class TestData
    {
        public static readonly List<AspNetUser> AspNetUsersList = new List<AspNetUser>
        {            
            new AspNetUser() { Id = "1", Email = "AO@peninsula-uk.com", PasswordHash = "111" },
            new AspNetUser() { Id = "2", Email = "SG@peninsula-uk.com", PasswordHash = "222" },
            new AspNetUser() { Id = "3", Email = "MA@peninsula-uk.com", PasswordHash = "333" }
        };
    }
}
