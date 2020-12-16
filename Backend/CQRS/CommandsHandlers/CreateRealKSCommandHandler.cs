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
            List<Problem> sortedProblems = expectedKnowledgeSpace.Problems.ToList();
            sortedProblems = sortedProblems.OrderBy(x => x.ProblemId).ToList();
            foreach (Problem problem in sortedProblems)
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
            var edgePairs = JsonConvert.DeserializeObject<List<List<int>>>(rawEdgesArray);
            edgePairs = removeTransitiveEdges(edgePairs);
            var map = new Dictionary<int, int>();
            int i = 0;
            foreach (Problem problem in sortedProblems)
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
        // Next to procedures implement an algorithm fo DFS
        // from each vertex. See: https://cs.stackexchange.com/a/29133 
        protected List<List<int>> removeTransitiveEdges(List<List<int>> edges)
        {
            List<List<int>> result = new List<List<int>>();
            foreach (var edge in edges)
            {
                List<List<int>> edgesCopy = new List<List<int>>();
                List<List<int>> tempCopy = edges.FindAll(x => x[0] == edge[0]);
                foreach (var temp in tempCopy)
                {
                    edgesCopy.AddRange(edges.FindAll(x => x[0] == temp[1]));
                }
                if (!isReachable(edges, edgesCopy, edge[1]))
                {
                    result.Add(edge.ToList());
                }
            }
            return result;
        }

        protected bool isReachable(List<List<int>> edges, List<List<int>> immediateEdges, int NumToReach)
        {
            foreach (var edge in immediateEdges)
            {
                if (edge[1] == NumToReach)
                {
                    return true;
                }
                List<List<int>> edgesCopy = edges.Select(edge => new List<int>(edge)).ToList();
                edgesCopy.RemoveAll(x => x[0] != edge[1]);
                if (isReachable(edges, edgesCopy, NumToReach))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
