using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Moq;
using NUnit.Framework;
using PBS.Client.API.Data.Queries;
using PBS.Client.API.Data.QueriesHandlers;
using PBS.Client.API.DataModel;
using PBS.Client.API.Models;
using PBS.Client.DataModel.Common;


namespace PBS.Client.API.Tests
{
    public static class TestData
    {
        public static readonly List<TBLCustomer> CustomerDataList = new List<TBLCustomer>
        {
           
            new TBLCustomer()
            {
               CustomerID = 111,
               CustomerKey = "AAA111",
               CompanyName = "Apple  Co."
            },

             new TBLCustomer()
            {
               CustomerID = 222,
               CustomerKey = "BBB222",
               CompanyName = "Banana Co."
            },

             new TBLCustomer()
            {
               CustomerID = 333,
               CustomerKey = "CCC33",
               CompanyName = "Cherry Co."
            }
        };
    }

    [TestFixture]
    public class QueryHandlerTests
    {
        private Mock<IClientDbContext> _mockDbContext;
 
        

        [TestFixtureSetUp]
        public void TestSetup()
        {

            // JOBS
            var mockJobs = new Mock<DbSet<TBLCustomer>>();
            var customerData = TestData.CustomerDataList.AsQueryable();

            mockJobs.As<IQueryable<TBLCustomer>>().Setup(m => m.Provider).Returns(customerData.Provider);
            mockJobs.As<IQueryable<TBLCustomer>>().Setup(m => m.Expression).Returns(customerData.Expression);
            mockJobs.As<IQueryable<TBLCustomer>>().Setup(m => m.ElementType).Returns(customerData.ElementType);
            mockJobs.As<IQueryable<TBLCustomer>>().Setup(m => m.GetEnumerator()).Returns(customerData.GetEnumerator);

          
            // DB CONTEXT
            _mockDbContext = new Mock<IClientDbContext>();
            _mockDbContext.Setup(m => m.Customers).Returns(mockJobs.Object);
        }

        [Test]
        public void Given_valid_client_When_called_Then_client_details_returned()
        {
            const int clientId = 222;

            GetClientDetailsByClientIDQueryHandler handler = new GetClientDetailsByClientIDQueryHandler(_mockDbContext.Object);

            GetClientDetailsByClientIDQuery qry = new GetClientDetailsByClientIDQuery()
            {
                ClientID = clientId
            };
            
            
            ClientDetailsViewModel model = handler.Execute(qry);

            Assert.IsNotNull( model );
            Assert.AreEqual(model.Id,           TestData.CustomerDataList[1].CustomerID );
            Assert.AreEqual(model.CAN,          TestData.CustomerDataList[1].CustomerKey);
            Assert.AreEqual(model.CompanyName,  TestData.CustomerDataList[1].CompanyName);          
        }
        
    }
}
