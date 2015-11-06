using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBS.Advice.API.Models
{
    public class ActionModel
    {
        public long ActionId { get; set; }
        public string AdviceGiven { get; set; }

        public long ActionTypeID { get; set; }

        public string ActionType { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }

        public string GeneralNotes { get; set; }
        public string InternalNotes { get; set; }

        public long? DocumentCount { get; set; }

        /*             
       
        public long JobID { get; set; }
         * 
        public Nullable<long> NatureOfAdviceID { get; set; }
        public string NatureOfAdviceOther { get; set; }
        
        public Nullable<long> IndemnityID { get; set; }
        public Nullable<long> IndemnityReasonID { get; set; }
        public string IndemnityOther { get; set; }
         * 
        public Nullable<long> ContactID { get; set; }
        * 
         * 
        public Nullable<System.DateTime> EditStartDate { get; set; }
        public Nullable<System.DateTime> EditCompletedDate { get; set; }
     
        public Nullable<System.DateTime> MeetingDate { get; set; }
        public Nullable<long> MeetingTypeID { get; set; }
      
        public string EmailAddress { get; set; }
        public Nullable<bool> EmailSent { get; set; }
        public string ExchangeGUID { get; set; }
        public Nullable<long> ResultingTaskId { get; set; }
        public string LoggedOnBehalfOf { get; set; }
        public Nullable<bool> Outbound { get; set; }
         * 
      
        
         * public string LastModifiedBy { get; set; }
        public Nullable<System.DateTime> LastModifiedDate { get; set; }
         * 
        public bool Deleted { get; set; }
        public string DeletedBy { get; set; }
        public Nullable<System.DateTime> DeletedDate { get; set; }
         * 
        public Nullable<bool> OutOfHours { get; set; }
        public Nullable<bool> ClientSatisfiedWithAdvice { get; set; }
        public Nullable<int> NumberOfEmployeesInvolved { get; set; }
        public Nullable<bool> IsKeyEvent { get; set; }
        public Nullable<long> PolicyNumberID { get; set; }
    
        public virtual Job Job { get; set; }
         
         */
    }
}