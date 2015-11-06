using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBS.Core.CQRS.Command
{
    public interface ICommandHandler<in TParameter> where TParameter : ICommand
    {
        void Execute(TParameter command);
    }

    public interface ICommandHandler<in TParameter, out TResult> where TParameter : ICommand
    {
        TResult Execute(TParameter command);
    }

   
}
