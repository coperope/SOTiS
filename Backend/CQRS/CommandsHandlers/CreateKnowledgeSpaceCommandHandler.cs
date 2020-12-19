using AutoMapper;
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
            List<List<int>> edgePairs = removeTransitiveEdges(request.Edges.Select(e => new List<int> { e.ProblemSourceId.Value, e.ProblemTargetId.Value }).ToList());
            List<Edge> cleanedEdges = new List<Edge>();
            foreach (var pair in edgePairs)
            {
                cleanedEdges.Add(new Edge()
                {
                    ProblemSourceId = pair[0],
                    ProblemTargetId = pair[1]
                });
            }
            KnowledgeSpace knowledgeSpace = new KnowledgeSpace
            {
                TestId = null,
                Problems = request.Problems,
                Edges = cleanedEdges,
                ProfessorId = request.UserId,
                Title = request.Title
            };
            var result = await _knowledgeSpaceRepository.CreateKnowledgeSpace(knowledgeSpace);
            return new CreateKnowledgeSpaceCommandResult
            {
                Id = result.KnowledgeSpaceId
            };
        }

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
