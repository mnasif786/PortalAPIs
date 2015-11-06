using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using Microsoft.Owin.Security.DataHandler.Encoder;
using PBS.AuthorisationServer.API.Entities;
using PBS.AuthorisationServer.API.Models;

namespace PBS.AuthorisationServer.API
{
    public static class ResourceStore
    {
        //public static ConcurrentDictionary<string, Resource> ResourcesList = new ConcurrentDictionary<string, Resource>();
        //private static readonly ApplicationDbContext _DbContext = null;

        static ResourceStore()
        {
           
            //var clientId = Guid.Parse("099153c2625149bc8ecb3e85e03f0022").ToString("N");
            //ResourcesList.TryAdd(clientId,
            //                    new Resource
            //                    {
            //                        ClientId = Guid.Parse("099153c2625149bc8ecb3e85e03f0022"),
            //                        Base64Secret = "IxrAjDoa2FqElO7IhrSrUJELhUckePEPVpaePlS_Xaw",
            //                        Name = "RS1"
            //                    });
        }

        public static Resource AddResource(string name)
        {
           
            
            var resourceId = Guid.NewGuid();//..ToString("N");

            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var newResource = new Resource { ResourceId = resourceId, Base64Secret = base64Secret, Name = name };
            //ResourcesList.TryAdd(clientId, newResource);

            var dbContext = ApplicationDbContext.GetDbContext();
            dbContext.Resources.Add(new Resource { ResourceId = resourceId, Base64Secret = base64Secret, Name = name });
            dbContext.SaveChanges();

            return newResource;
        }

        public static Resource FindResource(string resourceId)
        {
            var dbContext = ApplicationDbContext.GetDbContext(); 
            var parsedResourceId = Guid.Parse(resourceId);
            var resource = dbContext.Resources.SingleOrDefault(r => r.ResourceId == parsedResourceId);
            return resource;

            //_DbContext.Resources.
            //Resource resource;
            //return ResourcesList.TryGetValue(clientId, out resource) ? resource : null;
        }
    }
}