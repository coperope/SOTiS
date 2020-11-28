using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsHandlers
{
    public class TakeTestCommandHandler : IRequestHandler<TakeTestCommand, TakeTestCommandResult>
    {
        public Task<TakeTestCommandResult> Handle(TakeTestCommand request, CancellationToken cancellationToken)
        {   
            // Generate Enrolment. Loop over all answers and create Enrolment answers.
            throw new NotImplementedException();
        }
    }
}
