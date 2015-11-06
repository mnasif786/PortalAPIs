using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI;
using Moq;
using NUnit.Core;
using NUnit.Framework;
using PBS.Documents.API.Common;
using PBS.Documents.API.Controllers;

namespace PBS.Documents.API.Tests
{
    public static class TestData
    {
        public static readonly int ClientID = 123;


        public static readonly List<Document> DocumentDataList = new List<Document>
        {
            new Document() { DocumentId = 1},
            new Document() { DocumentId = 2},
            new Document() { DocumentId = 3}           
        };

        public static readonly Document ContentDocument = new Document()
        {
            DocumentId = 123,
         
            Caveats = "",
            Content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00 },
            Title = "ten byte pdf document",
            Extension = "pdf"
        };

        public static void AddClaimsPrincipal(this DocumentController controller, Dictionary<string, string> claims = null)
        {
            var cp = new Mock<ClaimsPrincipal>();

            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>()))
                .Returns((string key, string value) => Validate(key, value, claims));

            controller.User = cp.Object;
        }

        private static bool Validate(string key, string value, Dictionary<string, string> claims)
        {
            if (claims == null)
                return true;

            return (claims[key] == value);
        }   
    }

    [TestFixture]
    public class DocumentController_ClientDocTests
   {
       private Mock<IDocumentService> _mockDocumentService;
      
       [SetUp]
       public void Setup()
       {
           _mockDocumentService = new Mock<IDocumentService>();   
        
           _mockDocumentService
               .Setup( m => m.GetClientDocuments(TestData.ClientID))
               .Returns(TestData.DocumentDataList);

           _mockDocumentService
              .Setup(m => m.GetClientDocumentById(TestData.ClientID, 123))
              .Returns( TestData.ContentDocument);
          
       }  


       [Test]
       public void  Given_Valid_Client_When_calling_Controller_Then_Returns_Correct_Docs()
       {
           DocumentController controller = GetTarget();

           IHttpActionResult actionResult = controller.GetClientDocuments(TestData.ClientID);
           Assert.IsInstanceOf<OkNegotiatedContentResult<List<Document>>>(actionResult);

           OkNegotiatedContentResult<List<Document>> contentResult = actionResult as OkNegotiatedContentResult<List<Document>>;
           Assert.IsNotNull(contentResult);
           Assert.IsNotNull(contentResult.Content);

           List<Document> returned = contentResult.Content.ToList();

           Assert.AreNotEqual(0, TestData.DocumentDataList.Count);

           Assert.AreEqual(TestData.DocumentDataList.Count, returned.Count);
           Assert.AreEqual(TestData.DocumentDataList[0].DocumentId, returned[0].DocumentId);           
       }


       [Test]
       public void Given_Doc_Exists_For_Client_When_GetClientDocuments_without_claim_Then_Return_Unauthorised()
       {
           Dictionary<string, string> claims = new Dictionary<string, string>()
           {
               { PBS.Claims.ClaimTypes.PBSClientId, (TestData.ClientID + 1).ToString() } // only claim is for client other than test client
           };

           DocumentController controller = GetTarget(claims);
        

           IHttpActionResult actionResult = controller.GetClientDocuments(TestData.ClientID);

           Assert.IsInstanceOf<UnauthorizedResult>(actionResult);
       }


       [Test]
       public void Given_Valid_Client_When_calling_Controller_Then_Returns_Correct_Document()
       {
           Dictionary<string, string> claims = new Dictionary<string, string>()
           {
               { PBS.Claims.ClaimTypes.PBSClientId, (TestData.ClientID).ToString() }
           };
           DocumentController controller = GetTarget();

           IHttpActionResult actionResult = controller.GetClientDocumentById(TestData.ClientID, 123);
           Assert.IsInstanceOf<OkNegotiatedContentResult<Document>>(actionResult);

           OkNegotiatedContentResult<Document> contentResult = actionResult as OkNegotiatedContentResult<Document>;
           Assert.IsNotNull(contentResult);
           Assert.IsNotNull(contentResult.Content);

           Document returned = contentResult.Content;

           Assert.AreEqual(TestData.ContentDocument.Content.Length, returned.Content.Length);
       }

       [Test]
       public void Given_Doc_Exists_For_Client_When_GetClientDocument_without_claim_Then_Return_Unauthorised()
       {
           Dictionary<string, string> claims = new Dictionary<string, string>()
           {
               { PBS.Claims.ClaimTypes.PBSClientId, (TestData.ClientID + 1).ToString() } // only claim is for client other than test client
           };

           DocumentController controller = GetTarget(claims);


           IHttpActionResult actionResult = controller.GetClientDocumentById(TestData.ClientID, 123);

           Assert.IsInstanceOf<UnauthorizedResult>(actionResult);
       }

       private DocumentController GetTarget(Dictionary<string, string> claims = null)
       {
           DocumentController controller = new DocumentController(_mockDocumentService.Object);
           controller.AddClaimsPrincipal(claims);

           return controller;
       }   
    }
}
