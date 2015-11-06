using System;
using System.Data.Entity;
using PBS.PortalAdmin.API.DataModel;

namespace PBS.PortalAdmin.API.Common
{
    public class ApplicationDbContext : IApplicationDbContext 
    {
        private bool _disposed;
        private readonly PeninsulaPortalEntities _context;

        public ApplicationDbContext(PeninsulaPortalEntities context)
        {
            _context = context;
        }

        public PeninsulaPortalEntities Context
        {
            get { return _context;}
        }

       
        public DbSet<AspNetUser> Users
        {
            get { return _context.Set<AspNetUser>(); }

        }

        public DbSet<UserProfile> UserProfiles {
            get
            {
                return _context.Set<UserProfile>();
            }
        }

        public DbSet<PeninsulaClient> PeninsulaClients
        {
            get
            {
                return _context.Set<PeninsulaClient>();
                
            }
        }

        public DbSet<AspNetUserClaim> Claims {
            get
            {
                return _context.Set<AspNetUserClaim>(); 
                
            } 
        }

        public DbSet<PendingUser> PendingUsers {

            get
            {
                return _context.Set<PendingUser>();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}