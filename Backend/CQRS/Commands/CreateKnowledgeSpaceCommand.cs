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
    public class CreateKnowledgeSpaceCommand : ICommand, IRequest<CreateKnowledgeSpaceCommandResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.All;

        public int? UserId { get; set; }
        public int TestId { get; set; }
        public string Title { get; set; }
        public int ProfessorId { get; set; }
        public ICollection<Problem> Problems { get; set; }
        public ICollection<Edge> Edges { get; set; }
    }
}
