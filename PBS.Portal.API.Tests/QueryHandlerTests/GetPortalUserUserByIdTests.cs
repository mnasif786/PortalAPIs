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
    public class GetPortalUserUserByIdTests
    {
        private Mock<IApplicationDbContext> _mockDbContext;

        [SetUp]
        public void TestSetup()
        {
            //mocking PendingUser Entity
            var mockNewUsers = new Mock<DbSet<AspNetUser>>();
            var userData = GetTestUsers().AsQueryable();

            mockNewUsers.As<IQueryable<AspNetUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockNewUsers.As<IQueryable<AspNetUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockNewUsers.As<IQueryable<AspNetUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockNewUsers.As<IQueryable<AspNetUser>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator());

            // DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext
                .Setup(m => m.Users)
                .Returns(mockNewUsers.Object);
        }

        [TestCase("4b8086bb-6565-4ac9-b325-21c0f7c87fb8", typeof(UserViewModel))]
        public void Given_Valid_Guid_When_GetNewUserDetailsByGuid_Called_Returns_Model(string guid, Type resultingType)
        {
            var handler = new GetPortalUserByIdQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUserByIdQuery { UserId  = guid };

            var portalUser = handler.Execute(query);
            Assert.IsInstanceOf(resultingType, portalUser);
        }

        private List<AspNetUser> GetTestUsers()
        {
            return  new List<AspNetUser>
            {
                new AspNetUser { Id = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb8").ToString(), Email = "hubbabubba@chewing.gum"  },
                new AspNetUser { Id = Guid.Parse("4b8086bb-6565-4ac9-b325-21c0f7c87fb9").ToString(), Email = "hubbabubba1@chewing.gum"}
            };
        }
            
       
    }
}
