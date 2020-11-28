using Backend.CQRS.CommandsResults;
using Backend.Entities;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Commands
{
    public class TakeTestCommand : ICommand, IRequest<TakeTestCommandResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Student;

        public Test Test { get; set; }
    }
}
