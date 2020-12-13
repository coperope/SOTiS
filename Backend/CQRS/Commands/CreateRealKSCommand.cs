using Backend.CQRS.CommandsResults;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Commands
{
    public class CreateRealKSCommand : ICommand, IRequest<CreateRealKSCommandResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Professor;

        public int? UserId { get; set; }
        public int KnowledgeSpaceId { get; set; }
    }
}
