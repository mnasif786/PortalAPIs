using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBS.Core.DBContext
{
    public interface IDbContext<out TContext> where TContext : DbContext
    {
        TContext Context { get; }
    }
}
