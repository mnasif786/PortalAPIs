using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using PBS.Portal.API.Common;
using PBS.Portal.API.Data.Queries;
using PBS.Portal.API.Data.QueriesHandlers;
using PBS.Portal.API.DataModel;
using PBS.Portal.API.Models;
using PBS.Portal.API.Tests.CommandHandlerTests;

namespace PBS.Portal.API.Tests.QueryHandlerTests
{
    public static class TestData
    {
        public const  string TestPendingCAN = "DEMO002";

        public const string TestRegisteredCAN = "FAT04";
        public const int TestRegisteredClientID = 12345;

        public static readonly List<PendingUser> PendingUsersList = new List<PendingUser>
        {
            new PendingUser {   Id = Guid.NewGuid(), 
                                Email = "hubbabubba@chewing.gum", CAN = TestPendingCAN, FirstName = "Winnie", LastName = "Pooh" },           
        };

        public static readonly List<AspNetUser> AspNetUsers = new List<AspNetUser>
        {
            new AspNetUser {    Id = Guid.NewGuid().ToString(), 
                                UserName = "kerr.avon@liberator.com" }
        };

        public static readonly List<AspNetUserClaim> AspNetUserClaims = new List<AspNetUserClaim>
        {
            new AspNetUserClaim { Id = 123, 
                                UserId = AspNetUsers[0].Id,
                                ClaimType = "PBS.ClientId",
                                ClaimValue = TestRegisteredClientID.ToString(),
                                AspNetUser = AspNetUsers[0]
                                }
        };

        public static readonly List<PeninsulaClient> PeninsulaClients = new List<PeninsulaClient>
        {
            new PeninsulaClient(){CAN = TestRegisteredCAN, ClientId = TestRegisteredClientID}
        };
    }

    public class GetClientEntryPointQueryHandlerTests
    {
        private Mock<IApplicationDbContext> _mockDbContext;

        [SetUp]
        public void TestSetup()
        {
            //mocking PendingUser Entity
            var mockPendingUsers = new Mock<DbSet<PendingUser>>();
            var pendingUserData = TestData.PendingUsersList.AsQueryable();

            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.Provider).Returns(pendingUserData.Provider);
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.Expression).Returns(pendingUserData.Expression);
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.ElementType).Returns(pendingUserData.ElementType);
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.GetEnumerator()).Returns(pendingUserData.GetEnumerator());

            //mocking AspNetUserClaim Entity
            var mockAspNetUserClaims = new Mock<DbSet<AspNetUserClaim>>();
            var userClaimsData = TestData.AspNetUserClaims.AsQueryable();

            mockAspNetUserClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Provider).Returns(userClaimsData.Provider);
            mockAspNetUserClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Expression).Returns(userClaimsData.Expression);
            mockAspNetUserClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.ElementType).Returns(userClaimsData.ElementType);
            mockAspNetUserClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.GetEnumerator()).Returns(userClaimsData.GetEnumerator());

            //mocking AspNetUser Entity
            var mockAspNetUsers = new Mock<DbSet<AspNetUser>>();
            var usersData = TestData.AspNetUsers.AsQueryable();

            mockAspNetUsers.As<IQueryable<AspNetUser>>().Setup(m => m.Provider).Returns(usersData.Provider);
            mockAspNetUsers.As<IQueryable<AspNetUser>>().Setup(m => m.Expression).Returns(usersData.Expression);
            mockAspNetUsers.As<IQueryable<AspNetUser>>().Setup(m => m.ElementType).Returns(usersData.ElementType);
            mockAspNetUsers.As<IQueryable<AspNetUser>>().Setup(m => m.GetEnumerator()).Returns(usersData.GetEnumerator());

            //mocking PeninsulaClient Entity
            var mockPeninsulaClients = new Mock<DbSet<PeninsulaClient>>();
            var peninsulaClientData = TestData.PeninsulaClients.AsQueryable();

            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.Provider).Returns(peninsulaClientData.Provider);
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.Expression).Returns(peninsulaClientData.Expression);
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.ElementType).Returns(peninsulaClientData.ElementType);
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.GetEnumerator()).Returns(peninsulaClientData.GetEnumerator());

            // DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext
                .Setup(m => m.PendingUsers)
                .Returns(mockPendingUsers.Object);

            _mockDbContext
             .Setup(m => m.Claims)
             .Returns(mockAspNetUserClaims.Object);

            _mockDbContext
           .Setup(m => m.Users)
           .Returns(mockAspNetUsers.Object);

            _mockDbContext
               .Setup(m => m.PeninsulaClients)
               .Returns(mockPeninsulaClients.Object);
        }

       
        [Test]
        public void Given_User_In_Pending_Users_When_Called_Returns_URL()
        {
            string expectedURL = String.Format("{0}/{1}={2}", ConfigurationManager.AppSettings["RegistrationEmailURL"], "uid", TestData.PendingUsersList[0].Id.ToString());

            var query = new GetClientEntryPointQuery() { CAN = TestData.TestPendingCAN };

            var handler = new GetClientEntryPointQueryHandler(_mockDbContext.Object);
            
            EntryPointURLModel model = handler.Execute(query);

            Assert.IsNotNull( model );
            Assert.IsNotNull( model.URL );

            Assert.IsFalse( String.IsNullOrEmpty( model.URL ) );

            Assert.AreEqual(expectedURL, model.URL);
        }


        [Test]
        public void Given_User_In_Registered_Users_When_Called_Returns_URL()
        {
            string expectedURL = String.Format("{0}/{1}={2}", ConfigurationManager.AppSettings["RegistrationEmailURL"], "uid", TestData.AspNetUsers[0].Id);

            var query = new GetClientEntryPointQuery() { CAN = TestData.TestRegisteredCAN };

            var handler = new GetClientEntryPointQueryHandler(_mockDbContext.Object);

            EntryPointURLModel model = handler.Execute(query);

            Assert.IsNotNull(model);
            Assert.IsNotNull(model.URL);

            Assert.IsFalse(String.IsNullOrEmpty(model.URL));

            Assert.AreEqual(expectedURL, model.URL);
        }
    }
}
