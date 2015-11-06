using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PBS.AuthorisationServer.API.Entities
{
    public class UserProfiles
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Initials { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}