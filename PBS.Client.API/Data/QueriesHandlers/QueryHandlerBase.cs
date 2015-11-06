


using PBS.Client.DataModel.Common;

namespace PBS.Client.API.Data.QueriesHandlers
{
    public abstract class QueryHandlerBase
    {
        private readonly IClientDbContext _context ;
        
        protected QueryHandlerBase(IClientDbContext dbContext)
        {
            _context = dbContext;
        }

       
        public IClientDbContext Context
        {
            get { return _context; }
        }
    }
}