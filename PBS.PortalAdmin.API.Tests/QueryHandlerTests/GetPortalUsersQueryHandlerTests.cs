using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

using PBS.PortalAdmin.API.Common;
using PBS.PortalAdmin.API.Data.Queries;
using PBS.PortalAdmin.API.Data.QueriesHandlers;
using PBS.PortalAdmin.API.DataModel;

namespace PBS.PortalAdmin.API.Tests.QueryHandlerTests
{
    [TestFixture]
    public class GetPortalUsersByCanQueryHandlerTests
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
            var mockClaims  = new Mock<DbSet<AspNetUserClaim>>();
            var claimsData = GetAspNetUserClaims().AsQueryable();
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Provider).Returns(claimsData.Provider);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Expression).Returns(claimsData.Expression);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.ElementType).Returns(claimsData.ElementType);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.GetEnumerator()).Returns(claimsData.GetEnumerator);

            // DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext.Setup(m => m.PeninsulaClients).Returns(mockPeninsulaClients.Object);
            _mockDbContext.Setup(m => m.Claims).Returns(mockClaims.Object);
        }


        [Test]
        public void Given_CAN_When_Value_is_Demo1_then_Peninsula_Client_with_client_id_1_is_returned()
        {
            var handler = new GetPortalUsersQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUsersQuery() {CanOrCompanyName = "Demo1"};

           var userViewModelList =  handler.Execute(query);

           Assert.IsNotNull(userViewModelList);
           Assert.That(userViewModelList[0].ClientId, Is.EqualTo(1));
        }

        [Test]
        public void Given_CAN_When_Value_is_Demo1_then_only_one_user_is_returned()
        {
            var handler = new GetPortalUsersQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUsersQuery() { CanOrCompanyName = "Demo1" };

            var userViewModelList = handler.Execute(query);
            Assert.That(userViewModelList.Count, Is.EqualTo(1));
        }

        [Test]
        public void Given_CAN_When_Value_is_Demo2_then_two_users_are_returned()
        {
            var handler = new GetPortalUsersQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUsersQuery() { CanOrCompanyName = "Demo2" };

            var userViewModelList = handler.Execute(query);
            Assert.That(userViewModelList.Count, Is.EqualTo(2));
        }


        [Test]
        public void Given_Company_When_Value_is_BMW_then_Peninsula_Client_with_client_id_1_is_returned()
        {
            var handler = new GetPortalUsersQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUsersQuery() { CanOrCompanyName = "BMW" };

            var userViewModelList = handler.Execute(query);

            Assert.IsNotNull(userViewModelList);
            Assert.That(userViewModelList[0].ClientId, Is.EqualTo(1));
        }

        [Test]
        public void Given_Company_When_Name_is_Fatface_then_only_one_user_is_returned()
        {
            var handler = new GetPortalUsersQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUsersQuery() { CanOrCompanyName = "Fatface" };

            var userViewModelList = handler.Execute(query);
            Assert.That(userViewModelList.Count, Is.EqualTo(1));
        }

        [Test]
        public void Given_Company_When_Name_is_Levis_then_two_users_are_returned()
        {
            var handler = new GetPortalUsersQueryHandler(_mockDbContext.Object);
            var query = new GetPortalUsersQuery() { CanOrCompanyName = "Levis" };

            var userViewModelList = handler.Execute(query);
            Assert.That(userViewModelList.Count, Is.EqualTo(2));
        }

        private static IEnumerable<PeninsulaClient> GetPeninsulaClients()
        {

            var peninsulaClientsList = new List<PeninsulaClient>
           {
            new PeninsulaClient(){ClientId = 1, CAN ="Demo1", CompanyName = "BMW"},
            new PeninsulaClient(){ClientId = 2, CAN ="Demo2", CompanyName = "Levis"},
            new PeninsulaClient(){ClientId = 3, CAN ="Demo3", CompanyName = "FatFace"}
           };

            return peninsulaClientsList;
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
