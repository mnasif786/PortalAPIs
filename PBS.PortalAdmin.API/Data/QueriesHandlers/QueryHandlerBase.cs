
using PBS.PortalAdmin.API.Common;

namespace PBS.PortalAdmin.API.Data.QueriesHandlers
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