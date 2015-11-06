using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Data.QueriesHandlers;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Models;

namespace PBS.Portal.API.Tests.QueryHandlerTests
{

    [TestFixture]
    public class GetNewUserDetailsByGuidTests
    {
        private Mock<IApplicationDbContext> _mockDbContext;

        [SetUp]
        public void TestSetup()
        {
            //mocking PendingUser Entity
            var mockNewUsers = new Mock<DbSet<PendingUser>>();
            var userData = TestData.PendingUsersList.AsQueryable();

            mockNewUsers.As<IQueryable<PendingUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockNewUsers.As<IQueryable<PendingUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockNewUsers.As<IQueryable<PendingUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockNewUsers.As<IQueryable<PendingUser>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            // DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext
                .Setup(m => m.PendingUsers)
                .Returns(mockNewUsers.Object);
        }

        [TestCase("4b8086bb-6565-4ac9-b325-21c0f7c87fb8", typeof(RegisterViewModel))]
        [TestCase("4b8086bb-6565-4ac9-b325-21c0f7c87fb9", typeof(RegisterViewModel))]
        //[TestCase("4b8086bb-6565-4ac9-b325-21c0f7c87fb0", null)]
        public void Given_Valid_Guid_When_GetNewUserDetailsByGuid_Called_Returns_Model(string guid, Type resultingType)
        {
            var handler = new GetPendingUserDetailsByGuidQueryHandler(_mockDbContext.Object);
            var query = new GetPendingUserDetailsByGuidQuery() { PendingUserId = Guid.Parse(guid) };

            var pendingUser = handler.Execute(query);
            Assert.IsInstanceOf(resultingType, pendingUser);
        }

        public static class TestData
        {
            public static readonly List<PendingUser> PendingUsersList = new List<PendingUser>
            {
                new PendingUser { Id = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb8"), Email = "hubbabubba@chewing.gum", CAN = "CANCAN", FirstName = "Winnie", LastName = "Pooh" },
                new PendingUser { Id = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb9"), Email = "hubbabubba1@chewing.gum", CAN = "CANCAN1", FirstName = "Tigger", LastName = "Tiglon" },
            };
        }
    }
}
