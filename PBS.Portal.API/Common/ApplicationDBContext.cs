using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Common
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
            get { return _context; }
        }

        public DbSet<PendingUser> PendingUsers
        {
            get { return _context.Set<PendingUser>(); }
        }

        public DbSet<AspNetUser> Users
        {
            get { return _context.Set<AspNetUser>(); }

        }

        public DbSet<UserProfile> UserProfiles
        {
            get
            {
                return _context.Set<UserProfile>();
            }
        }


        public DbSet<Feedback> Feedback
        {
            get
            {
                return _context.Set<Feedback>();
            }
        }

        public DbSet<PeninsulaClient> PeninsulaClients
        {
            get
            {
                return _context.Set<PeninsulaClient>();

            }
        }

        public DbSet<AspNetUserClaim> Claims
        {
            get
            {
                return _context.Set<AspNetUserClaim>();

            }
        }

        public int Save()
        {
            return Context.SaveChanges();
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