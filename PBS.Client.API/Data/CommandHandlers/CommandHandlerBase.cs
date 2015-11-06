


using PBS.Client.DataModel.Common;

namespace PBS.Client.API.Data.CommandHandlers
{
    public abstract class CommandHandlerBase
    {
        private readonly IClientDbContext _context;

        public CommandHandlerBase(IClientDbContext context)
        {
            _context = context;
        }

        public IClientDbContext Context
        {
            get { return _context; }
        }

    }
}
