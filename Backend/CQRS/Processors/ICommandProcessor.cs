using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public interface ICommandProcessor
    {
        public abstract Task<ICommandResult> Execute(ICommand command);
    }
}
