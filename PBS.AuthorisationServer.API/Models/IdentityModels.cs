using System;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using PBS.AuthorisationServer.API.Entities;

namespace PBS.AuthorisationServer.API.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public virtual long? ProfileId { get; set; }
        public virtual UserProfiles Profile { get; set; }
        
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private static ApplicationDbContext applicationDbContext = null;

        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<UserProfiles> Profiles { get; set; }
       
        public ApplicationDbContext()
            : base("IdentityDbContext", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            // Set the database intializer which is run once during application start
            // This seeds the database with admin user credentials and admin role
            Database.SetInitializer<ApplicationDbContext>(null); // Set it to null to prevent database schema and data recreation.
        }

        public static ApplicationDbContext Create()
        {
            applicationDbContext = new ApplicationDbContext();
            return applicationDbContext;
        }

        public static ApplicationDbContext GetDbContext()
        {
            return applicationDbContext;
        }

      
    }
    
}