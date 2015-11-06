using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace PBS.Core.CQRS.Command
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IContainer _container;

        public CommandDispatcher(IContainer container)
        {
            _container = container; 
        }

        public void Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            
            dynamic handler = _container.GetInstance( typeof(ICommandHandler<TParameter>) );

            handler.Execute((dynamic) command);

        }

        public TResult Dispatch<TParameter, TResult>(TParameter command) where TParameter : ICommand
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }


            dynamic handler = _container.GetInstance(typeof(ICommandHandler<TParameter, TResult>));

           return  handler.Execute((dynamic)command);
        }
    }
}
