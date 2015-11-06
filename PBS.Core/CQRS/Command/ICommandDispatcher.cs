using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBS.Core.CQRS.Command
{
    public interface ICommandDispatcher
    {
        void Dispatch<TParameter>(TParameter command) where TParameter : ICommand;
        TResult Dispatch<TParameter,TResult>(TParameter command) where TParameter : ICommand;
    }
}
