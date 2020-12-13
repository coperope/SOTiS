using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsHandlers
{
    public class CreateRealKSCommandHandler : IRequestHandler<CreateRealKSCommand, CreateRealKSCommandResult>
    {
        private readonly IKnowledgeSpaceRepository _knowledgeSpaceRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        public CreateRealKSCommandHandler(IKnowledgeSpaceRepository knowledgeSpaceRepository, IHttpClientFactory httpClientFactory)
        {
            _knowledgeSpaceRepository = knowledgeSpaceRepository;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<CreateRealKSCommandResult> Handle(CreateRealKSCommand request, CancellationToken cancellationToken)
        {
            KnowledgeSpace expectedKnowledgeSpace = await _knowledgeSpaceRepository.GetSingleKnowledgeSpaceByIdWidthIncludes(request.KnowledgeSpaceId);
            List<string> problems = new List<string>();
            foreach (Problem problem in expectedKnowledgeSpace.Problems)
            {
                problems.Add(problem.ProblemId.ToString());
            }
            KnowledgeSpace realKnowledgeSpace = new KnowledgeSpace
            {
                ProfessorId = expectedKnowledgeSpace.ProfessorId,
                Title = "Real " + expectedKnowledgeSpace.Title,
                IsReal = true,
                KnowledgeSpaceId = expectedKnowledgeSpace.KnowledgeSpaceId
            };
            var content = new StringContent(problems.ToString());
            KnowledgeSpace createdRealKS = await _knowledgeSpaceRepository.CreateKnowledgeSpace(realKnowledgeSpace);
            HttpClient client = _httpClientFactory.CreateClient();
            var result = await client.PostAsync("http://localhost:8000/kst/iita", content);
            var a = result.Content;
            
            return new CreateRealKSCommandResult
            {
                Id = createdRealKS.KnowledgeSpaceId
            };
        }
    }
}
