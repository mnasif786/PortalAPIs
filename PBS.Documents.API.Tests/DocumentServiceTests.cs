using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Moq;
using NUnit.Framework;
using PBS.Documents.API.Common;
using PBS.Documents.API.Extensions;
using PBS.Documents.API.SharepointDocService;
using InstallationReceipt = PBS.Documents.API.SharepointDocService.InstallationReceipt;

namespace PBS.Documents.API.Tests
{
    // @todo StationaryDocumentsWithContent list breaks if you initialise it with a StationeryDocument not in the StationeryDocuemtns list but is that our problem...?
    [TestFixture]
    public class DocumentServiceTests
    {

        private Mock<ISpService> _iSpService;
        private SharepointDocumentService _sharepointDocumentService;
        private static readonly List<string> Locations = new List<string>() {"barney", "fred", "wilma", "betty"};

        private static readonly List<StationaryDocument> StationeryDocuments = new List<StationaryDocument>()
        {
            new StationaryDocument() { Id = 1, Location = Locations[0]},
            new StationaryDocument() { Id = 2, Location = Locations[1]},
            new StationaryDocument() { Id = 3, Location = Locations[2]},
            new StationaryDocument() { Id = 2, Location = Locations[1]},
            new StationaryDocument() { Id = 1, Location = Locations[0]}
        };

        private static readonly List<StationaryDocumentWithContent> StationaryDocumentsWithContent = new List<StationaryDocumentWithContent>()
        {
            new StationaryDocumentWithContent() { Document = StationeryDocuments[0], FBytes = new byte[] { 0x20 } },
            new StationaryDocumentWithContent() { Document = StationeryDocuments[1], FBytes = new byte[] { 0x20, 0x20 } },
            new StationaryDocumentWithContent() { Document = StationeryDocuments[2], FBytes = new byte[] { 0x20, 0x20, 0x20 } },
            new StationaryDocumentWithContent() { Document = StationeryDocuments[3], FBytes = new byte[] { 0x20, 0x20 } },
            new StationaryDocumentWithContent() { Document = StationeryDocuments[4], FBytes = new byte[] { 0x20} }
        };

        private static readonly List<InstallationReceipt> SharepointInstallationReceipts = new List<InstallationReceipt>()
        {
            new InstallationReceipt() { CustomerIdk__BackingField = "111", ClientApprovalk__BackingField = "Approved" },
            new InstallationReceipt() { CustomerIdk__BackingField = "112", ClientApprovalk__BackingField = "Approved" }
        };

        [SetUp]
        public void Setup()
        {
            _iSpService = new Mock<ISpService>();

            _iSpService
                .Setup(m => m.GetStationaryLocations())
                .Returns(Locations.ToArray);

            _iSpService
                .Setup(m => m.GetStationaryDocuments(It.IsAny<string>()))
                .Returns((string loc) => StationeryDocuments.Where(sd => sd.Location == loc).ToArray());

            _iSpService
                .Setup(m => m.GetStationaryDocument(It.IsAny<int>()))
                .Returns((int docId) => StationaryDocumentsWithContent.First(sdwc => sdwc.Document.Id == docId));

            _iSpService
                .Setup(m => m.GetAllInstallationReceipts("111"))
                .Returns(SharepointInstallationReceipts.ToArray());

            _sharepointDocumentService = new SharepointDocumentService(_iSpService.Object);
        }

        [Test]
        public void Sharepoint_Document_Service_Returns_All_Documents_Without_Any_Duplicates()
        {
            var allDocuments = _sharepointDocumentService.GetAllStationeryDocuments();
            Assert.AreEqual(3, allDocuments.Count);
        }

        [Test]
        public void Given_Client_Id_Sharepoint_Document_Servive_Returns_All_Installation_Receipts()
        {
            var allInstallationReceipts = _sharepointDocumentService.GetAllInstallationReceipts("111");
            Assert.AreEqual(2, allInstallationReceipts.Count);
        }


        #region integration tests
        [Test]
        public void Given_Array_Of_Sharepoint_Documents_With_Caveats_Extract_Distinct_Caveats_As_Object()
        {
            SharepointDocService.Document[] sharepointDocuments = {
                new SharepointDocService.Document { Caveats = "Caveat1Name:Caveat1Desc;Caveat2Name:Caveat2Desc;Caveat3Name:Caveat3Desc" },
                new SharepointDocService.Document { Caveats = "Caveat1Name:Caveat1Desc;Caveat2Name:Caveat2Desc" },
                new SharepointDocService.Document { Caveats = "Caveat1Name:Caveat1Desc" },

                new SharepointDocService.Document { Caveats = "Caveat1Name:Caveat1Desc" },
                new SharepointDocService.Document { Caveats = "Caveat1Name:Caveat1Desc" },
                new SharepointDocService.Document { Caveats = "Caveat1Name:Caveat1Desc" },
            };

            var integrationTestMakeDistinct = ModelExtensions.GetDistinctCaveatsFromDocuments(sharepointDocuments);
            Assert.AreEqual(3, integrationTestMakeDistinct.Length);
            integrationTestMakeDistinct.ToList().ForEach(i => Assert.AreEqual(typeof(Caveat), i.GetType()));
            integrationTestMakeDistinct.ToList().ForEach(i => Assert.AreEqual(i.Status, false));
        }
        #endregion integration tests

    }
}
