using AutoMapper;
using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using MediatR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsHandlers
{
    public class CreateRealKSCommandHandler : IRequestHandler<CreateRealKSCommand, CreateRealKSCommandResult>
    {
        private readonly IKnowledgeSpaceRepository _knowledgeSpaceRepository;
        private readonly IHttpClientFactory _httpClientFactory;
        private IMapper _mapper;
        public CreateRealKSCommandHandler(IKnowledgeSpaceRepository knowledgeSpaceRepository, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _knowledgeSpaceRepository = knowledgeSpaceRepository;
            _httpClientFactory = httpClientFactory;
            _mapper = mapper;
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
                ExpectedKnowledgeSpace = expectedKnowledgeSpace.KnowledgeSpaceId,
                Problems = new List<Problem>(),
                Edges = new List<Edge>()
            };
            var json = JsonConvert.SerializeObject(problems);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            
            KnowledgeSpace createdRealKS = await _knowledgeSpaceRepository.CreateKnowledgeSpace(realKnowledgeSpace);
            HttpClient client = _httpClientFactory.CreateClient();
            var result = await client.PostAsync("http://localhost:8000/kst/iita", content);
            var a = await result.Content.ReadAsStringAsync();

            dynamic rawEdgesArray = JsonConvert.DeserializeObject<dynamic>(a);
            var edgePairs = JsonConvert.DeserializeObject<int[][]>(rawEdgesArray);
            var map = new Dictionary<int, int>();
            int i = 0;
            foreach (Problem problem in expectedKnowledgeSpace.Problems)
            {
                int problemID = problem.ProblemId;
                Problem newProblem = new Problem();
                newProblem.ProblemId = new int();
                newProblem.KnowledgeSpaceId = createdRealKS.KnowledgeSpaceId;
                newProblem.X = problem.X;
                newProblem.Y = problem.Y;
                newProblem.Title = problem.Title;
                var savedProblem = await _knowledgeSpaceRepository.addProblem(newProblem);
                
                map.Add(i++, savedProblem.ProblemId);
            }

            foreach (var pair in edgePairs)
            {
                Edge edge = new Edge();
                edge.ProblemSourceId = map[pair[0]];
                edge.ProblemTargetId = map[pair[1]];
                edge.KnowledgeSpaceId = createdRealKS.KnowledgeSpaceId;
                var ret = await _knowledgeSpaceRepository.addEdge(edge);
            }
            
            return new CreateRealKSCommandResult
            {
                Id = createdRealKS.KnowledgeSpaceId
            };
        }

    }
}
