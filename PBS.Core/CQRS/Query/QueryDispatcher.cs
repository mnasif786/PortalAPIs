using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace PBS.Core.CQRS.Query
{
    public class QueryDispatcher: IQueryDispatcher
    {
        private readonly IContainer _container;

        public QueryDispatcher(IContainer container)
        {
            _container = container;
        }

        public TResult Dispatch<TResult>(IQuery<TResult> query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }

           // var handler = _container.GetInstance<IQueryHandler<IQuery<TResult>, TResult>>();
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType( query.GetType(), typeof(TResult) );
            dynamic handler = _container.GetInstance(handlerType);

            if (handler == null)
            {
                throw new Exception( "Unsupported Query Handler Type" );
            }

            return handler.Execute((dynamic) query);

        }
    }
}
