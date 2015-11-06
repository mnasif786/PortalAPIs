using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PBS.Core.DBContext;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Common
{
    public interface IApplicationDbContext : IDbContext<PeninsulaPortalEntities> , IDisposable
    {
        DbSet<AspNetUser> Users { get; }

        DbSet<PendingUser> PendingUsers { get; }

        DbSet<UserProfile> UserProfiles { get; }
        DbSet<Feedback> Feedback { get; }
        DbSet<PeninsulaClient> PeninsulaClients { get; }
        DbSet<AspNetUserClaim> Claims { get; }

        int Save();

    }
}