using PBS.Client.API.DataModel;
using PBS.Client.API.Models;

namespace PBS.Client.API.Helpers
{
    public static class ModelExtensions
    {        

        public static ClientDetailsViewModel ToClientDetailsViewModel(this TBLCustomer customer)
        {
            return new ClientDetailsViewModel()
            {
               Id = customer.CustomerID,
               CompanyName = customer.CompanyName,
               CAN = customer.CustomerKey
            };
        }

      

    }
}