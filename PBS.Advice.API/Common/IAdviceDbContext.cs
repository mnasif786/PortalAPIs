using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PBS.Advice.API.DataModel;
using Action = PBS.Advice.API.DataModel.Action;

namespace PBS.Advice.DataModel.Common
{
    public interface IAdviceDbContext : IDisposable
    {
        DbSet<Job> Jobs { get; }
        DbSet<Action> Actions { get; }

        void SaveChanges();
    }
}
