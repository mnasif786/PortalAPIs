using System;
using System.Data.Entity;
using PBS.Client.API.DataModel;
using PBS.Client.DataModel.Common;


namespace PBS.Client.API.Common
{

    public class ClientDbContext : IClientDbContext
    {
        protected readonly ClientEntities _Context;
        private bool _disposed;

        public ClientDbContext(ClientEntities contextClientEntities)
        {
            _Context = contextClientEntities;
        }

        public ClientEntities Context
        {
            get { return _Context; }
        }

        public DbSet<TBLCustomer> Customers
        {
            get { return _Context.Set<TBLCustomer>(); }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void SaveChanges()
        {
            _Context.SaveChanges();
        }
    }
}
