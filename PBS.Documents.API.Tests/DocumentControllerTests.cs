using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using PBS.Documents.API.Common;
using PBS.Documents.API.Controllers;
using PBS.Documents.API.SharepointDocService;
using Document = PBS.Documents.API.Common.Document;
using InstallationReceipt = PBS.Documents.API.Common.InstallationReceipt;

namespace PBS.Documents.API.Tests
{
    [TestFixture]
    public class DocumentControllerTests
    {
        private Mock<IDocumentService> _mockDocumentService;

        private DocumentController _controller;

        private readonly List<Document> _documentDataList = new List<Document>
        {
            new Document() { DocumentId = 1, Location = _locations[0]},
            new Document() { DocumentId = 2, Location = _locations[0]},
            new Document() { DocumentId = 3, Location = _locations[0]},
            new Document() { DocumentId = 4, Location = _locations[0]},
            new Document() { DocumentId = 5, Location = _locations[1]},
            new Document() { DocumentId = 6, Location = _locations[1]},
            new Document() { DocumentId = 7, Location = _locations[1]},
            new Document() { DocumentId = 8, Location = _locations[1]}
        };

        private static readonly List<string> _locations = new List<string>{ "barney", "fred", "wilma", "betty" };

        private static readonly List<Document> _barneyDocuments = new List<Document>
        {
            new Document() { DocumentId = 1, Location = _locations[0]},
            new Document() { DocumentId = 2, Location = _locations[0]},
            new Document() { DocumentId = 3, Location = _locations[0]},
            new Document() { DocumentId = 4, Location = _locations[0]},
            new Document() { DocumentId = 5, Location = _locations[0]}
        };

        private static readonly List<Document> _fredDocuments = new List<Document>
        {
            new Document() { DocumentId = 1, Location = _locations[1]},
            new Document() { DocumentId = 2, Location = _locations[1]},
            new Document() { DocumentId = 6, Location = _locations[1]},
            new Document() { DocumentId = 7, Location = _locations[1]},
            new Document() { DocumentId = 8, Location = _locations[1]}
        };

        private static readonly Document _contentDocument = new Document()
        {
            DocumentId = 123,
            Location = _locations[1],
            Caveats = "",
            Content = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x00 },
            Title = "",
            Extension = "pdf"
        };

        private  static readonly List<InstallationReceipt> InstallationReceipts = new List<InstallationReceipt>
        {
            new InstallationReceipt() { ClientId = "123"},
            new InstallationReceipt() { ClientId = "234"},
        };

        private IHttpActionResult _actionResult;

        [SetUp]
        public void Setup()
        {
            _mockDocumentService = new Mock<IDocumentService>();

            _mockDocumentService
                .Setup(m => m.GetStationeryLocations())
                .Returns(_locations);

            _mockDocumentService
                .Setup(m => m.GetStationeryDocumentsByLocation("barney"))
                .Returns(_barneyDocuments);

            _mockDocumentService            
                .Setup(m => m.GetStationeryDocumentsByLocation("fred"))
                .Returns(_fredDocuments);
                        
            _mockDocumentService
               .Setup(m => m.GetStationeryDocumentsByLocation("wilma"))
               .Returns(new List<Document>());

            _mockDocumentService
             .Setup(m => m.GetStationeryDocumentsByLocation("betty"))
             .Returns(new List<Document>());

            _mockDocumentService
                .Setup(m => m.GetStationeryDocumentById(123))
                .Returns(_contentDocument);

            _mockDocumentService
                .Setup(m => m.GetAllInstallationReceipts(It.IsAny<string>()))
                .Returns(InstallationReceipts);

            _mockDocumentService
                .Setup(m => m.GetAllInstallationReceipts("1"))
                .Returns(new List<InstallationReceipt>());

            _controller = new DocumentController(_mockDocumentService.Object);
            _controller.AddClaimsPrincipal(null);
        }

        [Test]
        public void Given_List_Of_Locations_Exists_When_Service_Called_Then_Locations_Returned()
        {
            _actionResult = _controller.GetStationeryLocations();
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<string>>>(_actionResult);


            OkNegotiatedContentResult<List<string>> contentResult = _actionResult as OkNegotiatedContentResult<List<string>>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            
            var locations = contentResult.Content;

            Assert.AreEqual(_locations.Count, locations.Count);
        }

        [Test]
        public void Given_A_Location_When_Service_Called_Then_Return_All_Documents_From_Location()
        {
            _actionResult = _controller.GetStationeryDocumentsByLocation("barney");
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<Document>>>(_actionResult);


            OkNegotiatedContentResult<List<Document>> contentResult = _actionResult as OkNegotiatedContentResult<List<Document>>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var documents = contentResult.Content;
            Assert.AreEqual(_barneyDocuments.Count, documents.Count);
        }

        [Test]
        public void Given_A_Document_Id_When_Service_Called_Then_Return_The_Document()
        {

            _actionResult = _controller.GetStationeryDocumentById(123);
            Assert.IsInstanceOf<OkNegotiatedContentResult<Document>>(_actionResult);

            OkNegotiatedContentResult<Document> contentResult = _actionResult as OkNegotiatedContentResult<Document>;
            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);

            var document = contentResult.Content;
            Assert.AreEqual(_contentDocument.Content.Length, document.Content.Length);

            // found this here: http://stackoverflow.com/questions/347818/using-moq-to-determine-if-a-method-is-called/347907#347907
            // does it make sense to anyone? apparently it just verifies the methods was called?
            // _mockDocumentService.Verify(m => m.GetDocumentById((1)));
        }

        [Test]
        public void Given_A_ClientId_Has_No_Installation_Receipts_When_Service_Called_Return_No_Installation_Receipts()
        {
            _actionResult = _controller.GetAllInstallationReceipts("1");
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<InstallationReceipt>>>(_actionResult);

            var contentResult = _actionResult as OkNegotiatedContentResult<List<InstallationReceipt>>;

            Assert.IsNotNull(contentResult);
            Assert.AreEqual(0, contentResult.Content.Count);
        }

        [Test]
        public void Given_A_ClientId_HAS_Installation_Receipts_When_Service_Called_Return_Installation_Receipts()
        {
            _actionResult = _controller.GetAllInstallationReceipts("2");
            Assert.IsInstanceOf<OkNegotiatedContentResult<List<InstallationReceipt>>>(_actionResult);

            var contentResult = _actionResult as OkNegotiatedContentResult<List<InstallationReceipt>>;
            Assert.IsNotNull(contentResult);
            Assert.IsTrue(contentResult.Content.Count > 1);
            Assert.AreEqual(InstallationReceipts.Count, contentResult.Content.Count);
        }
    }
}
