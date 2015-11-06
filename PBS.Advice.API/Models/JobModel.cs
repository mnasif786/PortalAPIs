using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Advice.API.DataModel;

namespace PBS.Advice.API.Models
{
    public static class Department
    {        
        public const long HealthAndSafety = 1;
        public const long Advice = 2;
        public const long Taxwise = 3;
    }

    public class JobModel
    {      
        public long JobId { get; set; }

        public long? ContactId { get; set; }

        public CustomerDetails CustomerDetails { get; set; }

        public int ClientID { get; set; }
        public string Subject { get; set; }

        public string CreatedBy { get; set; }

        public bool Closed { get; set; }

        public long? LastActionId { get; set; }

        public Nullable<System.DateTime> LastActionDate { get; set; }

        public System.DateTime CreatedDate { get; set; }

        //public List<ActionModel> Actions { get; set; }

        /*     
            public Nullable<long> DepartmentID { get; set; }
           
            public Nullable<long> AdviceCardID { get; set; }
            public Nullable<long> SiteAddressID { get; set; }       
            public Nullable<long> CurrentNatureOfAdviceID { get; set; }
              
            public Nullable<long> IndemnityID { get; set; }
            public bool Sensitive { get; set; }
            public string Checklist { get; set; }       
            
            public string LastModifiedBy { get; set; }
            public System.DateTime LastModifiedDate { get; set; }
            public string LastModifyingAction { get; set; }
            public string LastModifiedReason { get; set; }
            
         *  public bool Deleted { get; set; }
            public string DeletedBy { get; set; }
            public Nullable<System.DateTime> DeletedDate { get; set; }
         * 
            public Nullable<long> LastKeyNatureOfAdviceId { get; set; }
            public Nullable<long> PolicyNumberID { get; set; }
            public Nullable<bool> ProActiveCallBackCreated { get; set; }
        */

    }
}