using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PBS.Portal.API.Common;

namespace PBS.Portal.API.Data.CommandHandlers
{
    public abstract class CommandHandlerBase
    {
        private readonly IApplicationDbContext _context;

        public CommandHandlerBase(IApplicationDbContext context)
        {
            _context = context;
        }

        public IApplicationDbContext Context
        {
            get { return _context; }
        }

    }
}
