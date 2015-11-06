using System;
using System.Data.Entity;
using PBS.Advice.API.DataModel;
using PBS.Advice.DataModel.Common;
using Action = PBS.Advice.API.DataModel.Action;

namespace PBS.Advice.API.Common
{

    public class AdviceDbContext : IAdviceDbContext
    {
        protected readonly AdviceEntities _Context;
        private bool _disposed;

        public AdviceDbContext(AdviceEntities contextAdviceEntities)
        {
            _Context = contextAdviceEntities;
        }

        public AdviceEntities Context
        {
            get { return _Context; }
        }

        public DbSet<Job> Jobs
        {
            get { return _Context.Set<Job>(); }
        }

        public DbSet<Action> Actions
        {
            get { return _Context.Set<Action>(); }
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
