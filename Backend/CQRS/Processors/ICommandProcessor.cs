using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public interface ICommandProcessor
    {
        public abstract Task<ICommandResult> Execute(ICommand command, IHttpContextAccessor context);
    }
}
