using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using PBS.Documents.API.Extensions;
using PBS.Documents.API.SharepointDocService;

namespace PBS.Documents.API.Common
{
    public class Document
    {       
        public int DocumentId { get; set; }
        public string Location { get; set; }      

        public string Title { get; set;  }

        public string Caveats { get; set; }

        public byte[] Content { get; set; }
        public string Extension { get; set; }
        public DateTime DateUploaded { get; set; }
    }

    public class InstallationReceipt
    {
        public string ClientId { get; set; }
        public string ReceiptId { get; set; }
        public string Status { get; set; }
        public string DateGenerated { get; set; }
        public string CompletedBy { get; set; }
        public string CompletedDate { get; set; }
        public string ClientDocIds { get; set; }
        public Document[] ClientDocuments { get; set; }
        public Caveat[] DistinctCaveats { get; set; }
    }
}