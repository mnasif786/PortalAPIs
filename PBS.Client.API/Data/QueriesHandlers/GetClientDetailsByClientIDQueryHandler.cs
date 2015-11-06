
using System;
using System.Linq;

using PBS.Client.API.Data.Queries;
using PBS.Client.API.DataModel;
using PBS.Client.API.Helpers;
using PBS.Client.API.Models;
using PBS.Client.DataModel.Common;
using PBS.Core.CQRS.Query;



namespace PBS.Client.API.Data.QueriesHandlers
{
    
    public class GetClientDetailsByClientIDQueryHandler: QueryHandlerBase , IQueryHandler<GetClientDetailsByClientIDQuery, ClientDetailsViewModel>
    {
        public GetClientDetailsByClientIDQueryHandler(IClientDbContext dbContext)
            : base(dbContext)
        {

        }

        public ClientDetailsViewModel Execute(GetClientDetailsByClientIDQuery query)
        {   
            ClientDetailsViewModel model = new ClientDetailsViewModel();
        
            TBLCustomer customer = Context.Customers.SingleOrDefault( c => c.CustomerID.Equals(query.ClientID) );
            if( customer == null)
                throw new ArgumentException("No Such Client");
        

            return customer.ToClientDetailsViewModel();        
        }
    }
}


  
