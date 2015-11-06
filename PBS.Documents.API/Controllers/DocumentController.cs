using System.Reflection.Emit;
using System.Web.Http;
using System.Web.Http.Cors;
using PBS.Claims.Extensions;
using PBS.Documents.API.Common;


namespace PBS.Documents.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/documents")]
   // [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DocumentController : ApiController
    {
        private readonly IDocumentService _documentService;
        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        #region stationery documents
        [Route("stationery/locations")]
        public IHttpActionResult GetStationeryLocations()
        {
            return Ok(_documentService.GetStationeryLocations());
        }

        [Route("stationery/locations/{location}")]
        public IHttpActionResult GetStationeryDocumentsByLocation(string location)
        {
            return Ok(_documentService.GetStationeryDocumentsByLocation(location));
        }

        [Route("stationery/{documentId}")]
        public IHttpActionResult GetStationeryDocumentById(int documentId)
        {
            Document doc = _documentService.GetStationeryDocumentById(documentId);
            if (doc == null)
            {
                return NotFound();
            }

            return Ok(doc);
        }

        [Route("stationery")]
        public IHttpActionResult GetAllStationeryDocuments()
        {
            return Ok(_documentService.GetAllStationeryDocuments());
        }
        #endregion stationery documents


        #region client documents
        [Route("client/{clientID}")]
        public IHttpActionResult GetClientDocuments(int clientID)
        {
            if (!User.CanViewDocsForClientId(clientID))
            {
                return Unauthorized();
            }

            return Ok(_documentService.GetClientDocuments(clientID));
        }

        [Route("client/{clientID}/{documentID}")]
        public IHttpActionResult GetClientDocumentById(int clientID, int documentID)
        {
            if (!User.CanViewDocsForClientId(clientID))
            {
                return Unauthorized();
            }

            Document doc = _documentService.GetClientDocumentById(clientID, documentID);
            if (doc == null)
            {
                return NotFound();
            }

            return Ok(doc);
        }
        #endregion stationery documents

        #region installation receipts
        [Route("receipts/client/{clientId}")]
        public IHttpActionResult GetAllInstallationReceipts(string clientId)
        {
            if (!User.CanViewDocsForClientId(long.Parse(clientId)))
            {
                return Unauthorized();
            }

            return Ok(_documentService.GetAllInstallationReceipts(clientId));
        }

        [Route("receipt/{receiptId}/{clientId}/{docIds}/{status}")]
        public IHttpActionResult GetInstallationReceiptById(string receiptId, string clientId, string docIds, string status)
        {
            if (!User.CanViewDocsForClientId(long.Parse(clientId)))
            {
                return Unauthorized();
            }

            return Ok(_documentService.GetInstallationReceiptById(receiptId, clientId, docIds, status));
        }


        #endregion installation receipts
    }
}
