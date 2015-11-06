using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBS.Client.API.Models
{
    public class ClientDetailsViewModel
    {
        public long Id { get; set; }
        public string CompanyName { get; set; }
        public string CAN { get; set; }
    }
}