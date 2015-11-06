using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using PBS.Documents.API.SharepointDocService;

namespace PBS.Documents.API.Extensions
{

    public static class ModelExtensions
    {
       
        public static Common.Document ToCommonDocument(this SharepointDocService.StationaryDocument sharepointDoc)
        {
            return new Common.Document()
            {
                DocumentId = sharepointDoc.Id,

                Title = sharepointDoc.Title,

                Caveats = null,

                Location = sharepointDoc.Location,

                Content = null,
                
                Extension = sharepointDoc.Extension == null ? null : sharepointDoc.Extension.TrimStart('.'),
                
                DateUploaded = sharepointDoc.CreatedOn
            };
        }
          
        public static Common.Document ToCommonDocument(this SharepointDocService.StationaryDocumentWithContent sharepointDoc)
        {
            Common.Document comm = sharepointDoc.Document.ToCommonDocument();
            
            comm.Content = new byte[ sharepointDoc.FBytes.Count() ];
            Buffer.BlockCopy( sharepointDoc.FBytes, 0,comm.Content,0, sharepointDoc.FBytes.Count() );

            return comm;
        }

        public static Common.Document ToCommonDocument(this SharepointDocService.Document sharepointDoc)
        {
            return new Common.Document()
            {
                DocumentId = sharepointDoc.Id,

                Title = sharepointDoc.Title,

                Caveats = sharepointDoc.Caveats,

                Content = null,
                
                Extension = sharepointDoc.Extension == null ? null : sharepointDoc.Extension.TrimStart('.'),
                
                DateUploaded = sharepointDoc.CreatedOn              
            };
        }

        public static Common.Document ToCommonDocument(this SharepointDocService.DocumentWithContent sharepointDoc)
        {
            Common.Document comm = sharepointDoc.Document.ToCommonDocument();
            
            comm.Content = new byte[ sharepointDoc.FBytes.Count() ];
            Buffer.BlockCopy( sharepointDoc.FBytes, 0,comm.Content,0, sharepointDoc.FBytes.Count() );

            return comm;
        }

        public static Common.InstallationReceipt ToCommonInstallationReceipt(this SharepointDocService.InstallationReceipt spInstallationReceipt)
        {
            return new Common.InstallationReceipt
            {
                ClientId = spInstallationReceipt.CustomerIdk__BackingField,
                ReceiptId = spInstallationReceipt.ReceiptsIdk__BackingField,
                Status = spInstallationReceipt.ClientApprovalk__BackingField,
                DateGenerated = spInstallationReceipt.Createdk__BackingField,
                CompletedBy = spInstallationReceipt.Editork__BackingField,
                CompletedDate = spInstallationReceipt.Modifiedk__BackingField,
                ClientDocIds = spInstallationReceipt.ClientDocIdsk__BackingField,
                ClientDocuments = spInstallationReceipt.Documentk__BackingField?.Select(spd => spd.ToCommonDocument()).ToArray(),
                DistinctCaveats = spInstallationReceipt.Documentk__BackingField != null ? GetDistinctCaveatsFromDocuments(spInstallationReceipt.Documentk__BackingField) : null
            };
        }

        public static Caveat[] GetDistinctCaveatsFromDocuments(Document[] installationReceiptDocuments)
        {
            // before: { "Caveat1Name:Caveat1Desc", "Caveat1Name:Caveat1Desc", "Caveat2Name:Caveat2Desc", "Caveat1Name:Caveat1Desc;Caveat3Name:Caveat3Desc"}
            var caveats = installationReceiptDocuments
                .Select(receiptDocuments => receiptDocuments.Caveats)
                // during: deal with the multiple caveats in one string that Junaid spoke about
                .SelectMany(caveatsString => caveatsString.Split(';'))
                .Distinct()
                // during: { "Caveat1Name:Caveat1Desc", "Caveat2Name:Caveat2Desc", "Caveat3Name:Caveat3Desc"}
                .Select(c =>
                new Caveat()
                {
                    Status = false,
                    Name = c.Split(':')[0],
                    Description = c.Split(':')[1]
                })
                .ToArray();
            // after: { new Caveat { status = false, name = Caveat1Name, Desc = Caveat1Desc }, new Caveat { status = false, name = Caveat2Name, Desc = Caveat2Desc }, ... }

            return caveats;
        }
    }
}