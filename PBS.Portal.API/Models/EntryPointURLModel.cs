using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace PBS.Portal.API.Models
{
    public class EntryPointURLModel
    {
        public EntryPointURLModel(string url)
        {
            this.URL = url;
        }

        public string URL { get; set; }
    }
}