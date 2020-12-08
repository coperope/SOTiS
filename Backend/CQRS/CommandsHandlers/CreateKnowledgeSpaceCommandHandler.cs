using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsHandlers
{
    public class CreateKnowledgeSpaceCommandHandler : IRequestHandler<CreateKnowledgeSpaceCommand, CreateKnowledgeSpaceCommandResult>
    {
        private readonly IKnowledgeSpaceRepository _knowledgeSpaceRepository;
        public CreateKnowledgeSpaceCommandHandler(IKnowledgeSpaceRepository knowledgeSpaceRepository)
        {
            _knowledgeSpaceRepository = knowledgeSpaceRepository;
        }
        public async Task<CreateKnowledgeSpaceCommandResult> Handle(CreateKnowledgeSpaceCommand request, CancellationToken cancellationToken)
        {
            KnowledgeSpace knowledgeSpace = new KnowledgeSpace
            {
                TestId = null,
                Problems = request.Problems,
                Edges = request.Edges,
                ProfessorId = request.UserId,
                Title = request.Title
            };
            var result = await _knowledgeSpaceRepository.CreateKnowledgeSpace(knowledgeSpace);
            return new CreateKnowledgeSpaceCommandResult
            {
                Id = result.KnowledgeSpaceId
            };
        }
    }
}
