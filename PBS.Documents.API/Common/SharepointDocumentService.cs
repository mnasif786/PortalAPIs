using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PBS.Documents.API.Extensions;
using PBS.Documents.API.SharepointDocService;
using SPDocument = PBS.Documents.API.SharepointDocService.Document;


namespace PBS.Documents.API.Common
{
    public class SharepointDocumentService : IDocumentService
    {
        private readonly ISpService _spService = null;

        public SharepointDocumentService(ISpService service)
        {
            _spService = service;
        }

        public List<string> GetStationeryLocations()
        {
            return _spService.GetStationaryLocations().ToList();
        }

        // couldnt return a string[].ToList as a Task...
        public async Task<string[]> GetStationeryLocationsAsync()
        {
            return await _spService.GetStationaryLocationsAsync();
        } 

        public Document GetStationeryDocumentById(int documentId)
        {
            StationaryDocumentWithContent sdwc = _spService.GetStationaryDocument(documentId);

            return sdwc.ToCommonDocument();
        }

        public List<Document> GetAllStationeryDocuments()
        {
            var allStationeryDocuments = new List<Document>();

            GetStationeryLocations().ForEach(location =>
                allStationeryDocuments.AddRange(
                    GetStationeryDocumentsByLocation(location).Where(doc => DocumentIsUnique(allStationeryDocuments, doc))
                )       
            );

            return allStationeryDocuments;
        }

        public List<Document> GetStationeryDocumentsByLocation(string location)
        {
            return _spService.GetStationaryDocuments(location).Select(
                theStationaryDocument => theStationaryDocument.ToCommonDocument()                
            ).ToList();
        }

        public bool DocumentIsUnique(List<Document> documents, Document document)
        {
            return (documents.FirstOrDefault(d => d.DocumentId == document.DocumentId) == null);
        }

        public List<Document> GetClientDocuments(int clientID)
        {
            List<Document> docs = new List<Document>();

            SPDocument[] spDocs = _spService.GetDocuments( clientID.ToString() );

            foreach (SPDocument spdoc in spDocs)
            {
                docs.Add(spdoc.ToCommonDocument());
            }

            return docs;

        }

        public Document GetClientDocumentById(int clientId, int documentID)
        {
            DocumentWithContent doc = _spService.GetDocument(clientId.ToString(), documentID);

            return doc.ToCommonDocument();

        }

        public List<InstallationReceipt> GetAllInstallationReceipts(string clientId)
        {
            return _spService.GetAllInstallationReceipts(clientId)
                .Select(ir => ir.ToCommonInstallationReceipt())
                .Where(ir => ir.Status == "Approved" || ir.Status == "In Progress" || ir.Status == "Assistance Required")
                .ToList();
        }

        public InstallationReceipt GetInstallationReceiptById(string receiptId, string clientId, string docIds, string status)
        {
            var sharepointInstallationReceipt = new SharepointDocService.InstallationReceipt
            {
                ReceiptsIdk__BackingField = receiptId,
                CustomerIdk__BackingField = clientId,
                ClientDocIdsk__BackingField = docIds,
                ClientApprovalk__BackingField = status
            };

            var temp = _spService.GetInstallationReceiptById(sharepointInstallationReceipt);

            return _spService.GetInstallationReceiptById(sharepointInstallationReceipt).ToCommonInstallationReceipt();
        }
    }
}
