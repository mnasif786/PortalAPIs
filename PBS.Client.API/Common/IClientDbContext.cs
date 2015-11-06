using System;
using System.Data.Entity;
using PBS.Client.API.DataModel;
using PBS.Core.DBContext;

namespace PBS.Client.DataModel.Common
{
    public interface IClientDbContext : IDbContext<ClientEntities>, IDisposable
    {
        DbSet<TBLCustomer> Customers { get; }        

        void SaveChanges();
    }
}


