using System;
using System.Data.Entity;
using PBS.Core.DBContext;
using PBS.PortalAdmin.API.DataModel;

namespace PBS.PortalAdmin.API.Common
{
    public interface IApplicationDbContext : IDbContext<PeninsulaPortalEntities> , IDisposable
    {
        DbSet<AspNetUser> Users { get; }
        DbSet<UserProfile> UserProfiles { get; }
        DbSet<PeninsulaClient> PeninsulaClients { get; }
        DbSet<AspNetUserClaim> Claims { get; }
        DbSet<PendingUser> PendingUsers { get; }

    }
}