using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IMediator _mediator;

        public CommandProcessor(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ICommandResult> Execute(ICommand command)
        {

            // check if query accepts that role, else throw error


            var result = await _mediator.Send(command);

            return result as ICommandResult;
        }
    }
}
