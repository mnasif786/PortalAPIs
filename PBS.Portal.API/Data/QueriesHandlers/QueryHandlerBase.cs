using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PBS.Portal.API.Common;
using PBS.Portal.API.DataModel;

namespace PBS.Portal.API.Data.QueriesHandlers
{
    public abstract class QueryHandlerBase
    {
        private readonly IApplicationDbContext _context ;
        
        protected QueryHandlerBase(IApplicationDbContext dbContext)
        {
            _context = dbContext;
        }

       
        public IApplicationDbContext Context
        {
            get { return _context; }
        }
    }
}