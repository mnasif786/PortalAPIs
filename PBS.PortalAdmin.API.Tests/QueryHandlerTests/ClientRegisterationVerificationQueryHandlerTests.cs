using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Moq;
using NUnit.Framework;
using PBS.PortalAdmin.API.Common;
using PBS.PortalAdmin.API.Data.Queries;
using PBS.PortalAdmin.API.Data.QueriesHandlers;
using PBS.PortalAdmin.API.DataModel;

namespace PBS.PortalAdmin.API.Tests.QueryHandlerTests
{
    [TestFixture]
    public class ClientRegisterationVerificationQueryHandlerTests
    {
        private Mock<IApplicationDbContext> _mockDbContext;

        [TestFixtureSetUp]
        public void TestSetup()
        {
            //mocking Peninsula Client Entity
            var mockPeninsulaClients = new Mock<DbSet<PeninsulaClient>>();
            var peninsulaClientData = GetPeninsulaClients().AsQueryable();
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.Provider).Returns(peninsulaClientData.Provider);
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.Expression).Returns(peninsulaClientData.Expression);
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.ElementType).Returns(peninsulaClientData.ElementType);
            mockPeninsulaClients.As<IQueryable<PeninsulaClient>>().Setup(m => m.GetEnumerator()).Returns(peninsulaClientData.GetEnumerator);

            //mocking AspNetUserClaim Entity
            var mockClaims = new Mock<DbSet<AspNetUserClaim>>();
            var claimsData = GetAspNetUserClaims().AsQueryable();
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Provider).Returns(claimsData.Provider);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Expression).Returns(claimsData.Expression);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.ElementType).Returns(claimsData.ElementType);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.GetEnumerator()).Returns(claimsData.GetEnumerator);

            //mocking pending Users Entity
            var mockPendingUsers = new Mock<DbSet<PendingUser>>();
            var pendingUserData = GetPenindingUsers().AsQueryable();
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.Provider).Returns(pendingUserData.Provider);
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.Expression).Returns(pendingUserData.Expression);
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.ElementType).Returns(pendingUserData.ElementType);
            mockPendingUsers.As<IQueryable<PendingUser>>().Setup(m => m.GetEnumerator()).Returns(pendingUserData.GetEnumerator);

            //DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext.Setup(m => m.PeninsulaClients).Returns(mockPeninsulaClients.Object);
            _mockDbContext.Setup(m => m.Claims).Returns(mockClaims.Object);
            _mockDbContext.Setup(m => m.PendingUsers).Returns(mockPendingUsers.Object);
        }

        [Test]
        public void Given_Client_provided_CAN_When_CAN_is_register_with_claims_then_return_true()
        {
            var handler = new ClientRegisterationVerificationQueryHandler(_mockDbContext.Object);
            var query = new ClientRegisterationVerificationQuery { ClientIdOrCan = "Demo1" };

            var result = handler.Execute(query);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_Client_provided_CAN_When_CAN_is_register_with_Pending_Users_then_return_true()
        {
            var handler = new ClientRegisterationVerificationQueryHandler(_mockDbContext.Object);
            var query = new ClientRegisterationVerificationQuery { ClientIdOrCan = "pen1" };

            var result = handler.Execute(query);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_Client_provided_CAN_When_CAN_is_not_register_with_claims_and_Pending_Users_then_return_false()
        {
            var handler = new ClientRegisterationVerificationQueryHandler(_mockDbContext.Object);
            var query = new ClientRegisterationVerificationQuery { ClientIdOrCan = "pen3" };

            var result = handler.Execute(query);
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_Client_provided_ClientId_When_Client_is_register_with_claims_then_return_true()
        {
            var handler = new ClientRegisterationVerificationQueryHandler(_mockDbContext.Object);
            var query = new ClientRegisterationVerificationQuery { ClientIdOrCan = "1" };

            var result = handler.Execute(query);
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_Client_provided_ClientId_When_ClientId_is_not_register_with_claims_and_Pending_Users_then_return_false()
        {
            var handler = new ClientRegisterationVerificationQueryHandler(_mockDbContext.Object);
            var query = new ClientRegisterationVerificationQuery { ClientIdOrCan = "6" };

            var result = handler.Execute(query);
            Assert.IsFalse(result);
        }

        private static IEnumerable<PeninsulaClient> GetPeninsulaClients()
        {

           var peninsulaClientsList = new List<PeninsulaClient>
           {
               new PeninsulaClient(){ClientId = 1, CAN ="Demo1", CompanyName = "BMW"},
               new PeninsulaClient(){ClientId = 2, CAN ="Demo2", CompanyName = "Levis"},
               new PeninsulaClient(){ClientId = 3, CAN ="Demo3", CompanyName = "FatFace"},
               new PeninsulaClient(){ClientId = 4, CAN ="Pen1", CompanyName = "Peninsula-UK"},
               new PeninsulaClient(){ClientId = 5, CAN ="pen2", CompanyName = "Peninsula-IRE"},
               new PeninsulaClient(){ClientId = 6, CAN ="pen3", CompanyName = "Peninsula"}
           };

            return peninsulaClientsList;
        }

        private static IEnumerable<PendingUser> GetPenindingUsers()
        {

            var pendingUsersList = new List<PendingUser>
           {
               new PendingUser(){ CAN ="Pen1", FirstName = "Nauman"},
               new PendingUser(){ CAN ="Pen2", FirstName = "Asif"}
           };

            return pendingUsersList;
        }

        private static IEnumerable<AspNetUserClaim> GetAspNetUserClaims()
        {
            var claimsList = new List<AspNetUserClaim> 
                {
                        new AspNetUserClaim()
                        {
                            ClaimType = "PBS.ClientId", 
                        ClaimValue = "1", 
                        UserId = "1", 
                        AspNetUser = new AspNetUser
                        {
                            Id = "1", Email = "sc@sc.com", ProfileId = 1, UserProfile = new UserProfile {Id = 1, FirstName = "Nauman", LastName = "Asif"}
                        }
                },

                new AspNetUserClaim()
                {
                    ClaimType = "PBS.ClientId", 
                    ClaimValue = "2", 
                    UserId = "2" , 
                    AspNetUser = new AspNetUser
                    {
                        Id = "2", Email = "sc@sc.com", ProfileId = 2, UserProfile = new UserProfile {Id = 2, FirstName = "Muhammad", LastName = "Khan"}
                    }
                },

                new AspNetUserClaim()
                {
                    ClaimType = "PBS.ClientId", ClaimValue = "3", 
                    UserId = "3" , 
                    AspNetUser = new AspNetUser{ Id = "3", Email = "bta@bt.com", ProfileId = 3, UserProfile = new UserProfile {Id = 3, FirstName = "Ibrahim", LastName = "Rana"}}
                },

                new AspNetUserClaim()
                {
                    ClaimType = "PBS.ClientId", 
                    ClaimValue = "2", 
                    UserId = "4" , 
                    AspNetUser = new AspNetUser{ Id = "3", Email = "aa@aa.com", ProfileId = 4, UserProfile = new UserProfile {Id = 4, FirstName = "John", LastName = "Mike"}}
                }
            };
            return claimsList;
        }


    }
}
