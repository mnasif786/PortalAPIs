using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Advice.API.DataModel;
using PBS.Advice.API.Models;
using Action = PBS.Advice.API.DataModel.Action;

//using Action = PBS.Advice.DataModel.Action;


namespace PBS.Advice.API.Helpers
{
    public static class ModelExtensions
    {
        public static ActionModel ToActionModel(this Action action)
        {
            return new ActionModel()
            {
                ActionId = action.ActionID,
                AdviceGiven = action.AdviceGiven,
                ActionTypeID = action.ActionType.ActionTypeID,
                ActionType = action.ActionType.Description,

                StartDate = action.StartDate,
                CompletedDate = action.CompletedDate,
                GeneralNotes = action.GeneralNotes,
                InternalNotes = action.InternalNotes,
                DocumentCount = action.DocumentCount,
                CreatedBy = action.CreatedBy,
                CreatedDate = action.CreatedDate,
            };
        }

        public static JobModel ToJobModel(this Job job)
        {
            return new JobModel()
            {
                JobId = job.JobID,
                ContactId = job.ContactID,

                Subject = job.Subject,
                CreatedBy = job.CreatedBy,
                CreatedDate = job.CreatedDate,
                Closed = job.Closed,
                LastActionId = job.LastActionId,
                LastActionDate = job.LastActionDate,
                ClientID = job.ClientID,  
                               
                CustomerDetails = GetCustomerDetails(job.ClientID)
            };
        }


        
        // BIG NASTY DIRTY HACK     
        // Until we have a Client API looking up customer details in the Peninsula DB, this will allow
        // the UI to show the CAN based on a hard coded list
        // SGG
        private static  List<CustomerDetails> customerDetails = new List<CustomerDetails>()
            {
                new CustomerDetails(){ ID = 652,     CompanyName = "Age Concern Gateshead Ltd"     ,CAN = "AGE27"       },
                new CustomerDetails(){ ID = 98341,   CompanyName = "K & A Motor Repairs Ltd"       ,CAN = "KAN016"      },
                new CustomerDetails(){ ID = 97252,   CompanyName = "Image on Food Ltd"             ,CAN = "IMA035"      },
                new CustomerDetails(){ ID = 77349,   CompanyName = "Spice Application Systems Ltd" ,CAN = "SPI055"      },
                new CustomerDetails(){ ID = 33749,   CompanyName = "Intranet Demonstration"        ,CAN = "DEMO002"     },
                new CustomerDetails(){ ID = 23604,   CompanyName = "Sika Ltd"                      ,CAN =  "SIK01"      },

                new CustomerDetails(){ ID = 76586,      CompanyName = "Trio Property Group"            ,CAN =  "MID130"    },
                new CustomerDetails(){ ID = 6845,       CompanyName = "J C & A Solicitors"             ,CAN =  "CUN02"     },
                new CustomerDetails(){ ID = 67229,      CompanyName = "Moran Logistics"                ,CAN =  "ARM042"    } 
            }; 

        private static CustomerDetails GetCustomerDetails(int clientID)
        {            
            return customerDetails.FirstOrDefault(c => c.ID == clientID);
        }
    }
}