using Backend.CQRS.CommandsResults;
using Backend.Utils.Enums;
using MediatR;

namespace Backend.CQRS.Commands
{
    public interface ICommand : IRequest<ICommandResult>
    {
        protected CQRSRole Permission { get; }
    }
}
