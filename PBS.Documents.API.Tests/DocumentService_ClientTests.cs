using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using PBS.Documents.API.Common;
using PBS.Documents.API.SharepointDocService;
using SPDocument = PBS.Documents.API.SharepointDocService.Document;
using SPDocumentWithContent = PBS.Documents.API.SharepointDocService.DocumentWithContent;


namespace PBS.Documents.API.Tests
{
    [TestFixture]
    public class DocumentService_ClientTests
    {
        private Mock<ISpService> _iSpService;
        private SharepointDocumentService _sharepointDocumentService;

        private const int ClientId = 123;
        private const int DocumentId = 2;
        private const int DocumentWithContentId = 10;

        private readonly List<SPDocument> _sharepointDocumentList = new List<SPDocument>
        {
            new SPDocument()  { Id = 1, Title = "Doc one",      Caveats ="" },            
            new SPDocument()  { Id = 2, Title = "Doc two",      Caveats ="" },           
            new SPDocument()  { Id = 3, Title = "Doc three",    Caveats ="" }            
        };

        private readonly SPDocumentWithContent _sharepointDocumentWithContent = new SPDocumentWithContent()
        {
            Document = new SPDocument() { Id = 10, Title = "Content Doc one", Caveats = "" },     

            FBytes = new byte[]{ 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }
            
            
        };


        [SetUp]
        public void Setup()
        {
            _iSpService = new Mock<ISpService>();

            _iSpService
                .Setup(m => m.GetDocuments(ClientId.ToString()))
                .Returns(_sharepointDocumentList.ToArray());


            _iSpService
              .Setup(m => m.GetDocument(ClientId.ToString(), DocumentWithContentId))
              .Returns(_sharepointDocumentWithContent);

            _sharepointDocumentService = new SharepointDocumentService(_iSpService.Object);
        }

        [Test]
        public void Test_IspService_Get_Client_Documents()
        {
            List<PBS.Documents.API.Common.Document> docs = _sharepointDocumentService.GetClientDocuments(ClientId);

            Assert.IsNotEmpty(docs);
            Assert.AreEqual(_sharepointDocumentList.Count, docs.Count);

            Assert.AreEqual(_sharepointDocumentList[0].Id, docs[0].DocumentId);
            Assert.AreEqual(_sharepointDocumentList[1].Id, docs[1].DocumentId);
            Assert.AreEqual(_sharepointDocumentList[2].Id, docs[2].DocumentId);
        }

        [Test]
        public void Test_IspService_Get_Client_Document_By_Id()
        {
            Common.Document commonDoc = _sharepointDocumentService.GetClientDocumentById(ClientId, DocumentWithContentId);

            Assert.IsNotNull(commonDoc);

            Assert.AreEqual(DocumentWithContentId, commonDoc.DocumentId);

            Assert.AreEqual(_sharepointDocumentWithContent.Document.Title, commonDoc.Title);
            Assert.AreEqual(_sharepointDocumentWithContent.Document.Caveats, commonDoc.Caveats);

            Assert.AreEqual(_sharepointDocumentWithContent.FBytes.Count(), commonDoc.Content.Count());
            for (int i = 0; i < commonDoc.Content.Count(); i++)
            {
                Assert.AreEqual(_sharepointDocumentWithContent.FBytes[i], commonDoc.Content[i]);
            }
        }

        private IDocumentService GetTarget()
        {
            return new SharepointDocumentService(_iSpService.Object);
        }

    }
}
