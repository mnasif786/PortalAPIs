using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using PBS.Documents.API.SharepointDocService;

namespace PBS.Documents.API.Common
{
    public interface IDocumentService
    {
        List<string> GetStationeryLocations();

        Document GetStationeryDocumentById(int documentId);

        List<Document> GetAllStationeryDocuments();

        List<Document> GetStationeryDocumentsByLocation(string location);

        List<Document> GetClientDocuments(int clientID);

        Document GetClientDocumentById(int clientId, int documentID);

        List<InstallationReceipt> GetAllInstallationReceipts(string clientId);

        // push an object in instead
        InstallationReceipt GetInstallationReceiptById(string receiptId , string clientId, string docIds, string status);
    }
}
