using System;
using System.Collections.Generic;
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

namespace PBS.Portal.API.Tests.QueryHandlerTests
{
    
    [TestFixture]
    public class GetUserProfileByUserNameNameQueryHandlerTests
    {

        private Mock<IApplicationDbContext> _mockDbContext;

        [TestFixtureSetUp]
        public void TestSetup()
        {            
            //mocking AspNetUserClaim Entity
            var mockClaims  = new Mock<DbSet<AspNetUserClaim>>();
            var claimsData = TestData.ClaimsList.AsQueryable();
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Provider).Returns(claimsData.Provider);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.Expression).Returns(claimsData.Expression);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.ElementType).Returns(claimsData.ElementType);
            mockClaims.As<IQueryable<AspNetUserClaim>>().Setup(m => m.GetEnumerator()).Returns(claimsData.GetEnumerator);

            //mocking AspNetUserClaim Entity
            var mockUsers= new Mock<DbSet<AspNetUser>>();
            var userData = TestData.UserList.AsQueryable();
            mockUsers.As<IQueryable<AspNetUser>>().Setup(m => m.Provider).Returns(userData.Provider);
            mockUsers.As<IQueryable<AspNetUser>>().Setup(m => m.Expression).Returns(userData.Expression);
            mockUsers.As<IQueryable<AspNetUser>>().Setup(m => m.ElementType).Returns(userData.ElementType);
            mockUsers.As<IQueryable<AspNetUser>>().Setup(m => m.GetEnumerator()).Returns(userData.GetEnumerator);


            // DB CONTEXT
            _mockDbContext = new Mock<IApplicationDbContext>();
            _mockDbContext.Setup(m => m.Users).Returns(mockUsers.Object);
            _mockDbContext.Setup(m => m.Claims).Returns(mockClaims.Object);
        }


        [Test]
        public void Given_User_When_With_CAN_Claim_available_then_Model_returns()
        {
            var handler = new GetUserProfileByUserNameQueryHandler(_mockDbContext.Object);
            var query = new GetUserProfileByUserNameQuery() { UserName = "Winnie@thepooh.com" };


           UserViewModel userViewModel =  handler.Execute(query);

           Assert.IsNotNull(userViewModel);

           Assert.AreEqual(1234, userViewModel.ClientId);
        }




        public  static class TestData
        {                      
            public static readonly List<AspNetUserClaim> ClaimsList = new List<AspNetUserClaim>
            {
                new AspNetUserClaim()
                {
                    UserId = "1",
                    ClaimType = "PBS.ClientId", 
                    ClaimValue = "1234"                 
                }         
            };

            public static readonly List<AspNetUser> UserList = new List<AspNetUser>
            {
                new AspNetUser()
                {   
                    Id = "1",
                    AspNetUserClaims = ClaimsList,

                    UserProfile = new UserProfile()
                    {
                        FirstName = "Winnie",
                        MiddleName = "Ther",
                        LastName = "Pooh"
                    },

                    UserName = "Winnie@thepooh.com"

                }         
            }; 

        }
    }

}
