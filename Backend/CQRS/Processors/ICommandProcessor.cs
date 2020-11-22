using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;

namespace Backend.CQRS.Processors
{
    public interface ICommandProcessor
    {
        public abstract ICommandResult Execute(ICommand command);
    }
}
