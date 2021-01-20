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
        private IPossibleStatesWithPossibilitiesRepository _possibleStatesWithPossibilitiesRepository;
        private IMapper _mapper;
        public CreateRealKSCommandHandler(IKnowledgeSpaceRepository knowledgeSpaceRepository,
            IPossibleStatesWithPossibilitiesRepository possibleStatesWithPossibilitiesRepository, IHttpClientFactory httpClientFactory, IMapper mapper)
        {
            _knowledgeSpaceRepository = knowledgeSpaceRepository;
            _httpClientFactory = httpClientFactory;
            _possibleStatesWithPossibilitiesRepository = possibleStatesWithPossibilitiesRepository;
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
            var reverseMap = new Dictionary<int, int>();
            var reverseMapForStates = new Dictionary<int, int>();
            int i = 0;
            List<int> problemIds = new List<int>();
            foreach (Problem problem in sortedProblems)
            {
                Problem newProblem = new Problem();
                newProblem.ProblemId = new int();
                newProblem.KnowledgeSpaceId = createdRealKS.KnowledgeSpaceId;
                newProblem.X = problem.X;
                newProblem.Y = problem.Y;
                newProblem.Title = problem.Title;
                var savedProblem = await _knowledgeSpaceRepository.addProblem(newProblem);

                reverseMap.Add(problem.ProblemId, i);
                map.Add(i++, savedProblem.ProblemId);
                reverseMapForStates.Add(savedProblem.ProblemId, i);
                problemIds.Add(savedProblem.ProblemId);
            }
            List<List<int>> edgePairsMapped = new List<List<int>>();
            int[,] levenshteinMatrixReal = new int[problemIds.Count(), problemIds.Count()];
            int[,] levenshteinMatrixExpected = new int[problemIds.Count(), problemIds.Count()];
            foreach (var pair in edgePairs)
            {
                Edge edge = new Edge();
                edge.ProblemSourceId = map[pair[0]];
                edge.ProblemTargetId = map[pair[1]];
                edge.KnowledgeSpaceId = createdRealKS.KnowledgeSpaceId;
                List<int> edgeMapped = new List<int>();
                edgeMapped.Add(map[pair[0]]);
                edgeMapped.Add(map[pair[1]]);
                edgePairsMapped.Add(edgeMapped);
                var ret = await _knowledgeSpaceRepository.addEdge(edge);
                levenshteinMatrixReal[pair[0], pair[1]] = 1;

            }
            foreach (var edge in expectedKnowledgeSpace.Edges.ToList())
            {
                levenshteinMatrixExpected[reverseMap[edge.ProblemSourceId.Value], reverseMap[edge.ProblemTargetId.Value]] = 1;
            }
            KnowledgeSpace ksWithAllStates = await findAllPossibleKnowledgeStates(edgePairsMapped, problemIds, realKnowledgeSpace.KnowledgeSpaceId);
            determinePosibilitiesForAllStates(ksWithAllStates, realKnowledgeSpace.KnowledgeSpaceId, reverseMapForStates, client);
            return new CreateRealKSCommandResult
            {
                Id = createdRealKS.KnowledgeSpaceId,
                levenshteinMatrixReal = levenshteinMatrixReal,
                levenshteinMatrixExpected = levenshteinMatrixExpected
            };
        }
        // Next two procedures implement an algorithm for DFS
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

        protected async Task<KnowledgeSpace> findAllPossibleKnowledgeStates(List<List<int>> edges, List<int> allProblemIds, int knowledgeSpaceId)
        {
            Random rnd = new Random();
            KnowledgeSpace ksWithAllStates = new KnowledgeSpace();
            ksWithAllStates.Title = "All posible knowledge states for KS: " + knowledgeSpaceId.ToString();
            ksWithAllStates.IsReal = false;
            ksWithAllStates.Problems = new List<Problem>();
            ksWithAllStates.Edges = new List<Edge>();
            ksWithAllStates.ExpectedKnowledgeSpace = knowledgeSpaceId;
            ksWithAllStates = await _knowledgeSpaceRepository.CreateKnowledgeSpace(ksWithAllStates);
            Problem allStatesProblem = new Problem();
            string headNodeTitle = string.Join(" ", allProblemIds);
            allStatesProblem.Title = headNodeTitle;
            allStatesProblem.KnowledgeSpaceId = ksWithAllStates.KnowledgeSpaceId;
            allStatesProblem.X = 600;
            allStatesProblem.Y = 180;
            allStatesProblem = await _knowledgeSpaceRepository.addProblem(allStatesProblem);
            List<int> leaves = findLeavesAmongEdges(edges, allProblemIds);
            int i = 0;
            foreach (int leaf in leaves)
            {
                List<int> remainingNodes = allProblemIds.Select(id => id).ToList();
                remainingNodes.Remove(leaf);
                createSubPossibleKStates(ksWithAllStates, edges, remainingNodes, allStatesProblem, leaf, rnd, allProblemIds.Count(), i, leaves.Count());
                i++;
            }
            return await _knowledgeSpaceRepository.GetSingleKnowledgeSpaceByIdWidthIncludes(ksWithAllStates.KnowledgeSpaceId);
        }
        protected async void createSubPossibleKStates(KnowledgeSpace ksWithAllStates, List<List<int>> edges, List<int> remainingPreviousNodes, Problem previousProblem, int previousLeaf, Random rnd, int leavesCount, int leaveIndex, int remainingLeavesCount)
        {
            List<List<int>> remainingEdges = edges.FindAll(x =>
            {
                return x[0] != previousLeaf && x[1] != previousLeaf;
            });
            string newNodeTitle = remainingPreviousNodes.Count() == 0 ? "EmptyNode" + ksWithAllStates.KnowledgeSpaceId : string.Join(" ", remainingPreviousNodes);
            Problem someStateProblem = _knowledgeSpaceRepository.getProblemByTitle(newNodeTitle);
            
            if(someStateProblem == null)
            {
                someStateProblem = new Problem();
                someStateProblem.Title = newNodeTitle;
                someStateProblem.KnowledgeSpaceId = ksWithAllStates.KnowledgeSpaceId;
                int multiplier = remainingLeavesCount == 1 ? 0 : ((leaveIndex + 1) / remainingLeavesCount < 0.5 ? (1 + leaveIndex - remainingLeavesCount) : (1 + leaveIndex - remainingLeavesCount / 2));
                someStateProblem.X = 600 + multiplier * 180;
                someStateProblem.Y = 180 + (leavesCount - remainingPreviousNodes.Count())*200;
                someStateProblem = await _knowledgeSpaceRepository.addProblem(someStateProblem);
            }
            
            Edge edge = new Edge();
            edge.KnowledgeSpaceId = ksWithAllStates.KnowledgeSpaceId;
            edge.ProblemSourceId = someStateProblem.ProblemId;
            edge.ProblemTargetId = previousProblem.ProblemId;
            edge = await _knowledgeSpaceRepository.addEdge(edge);
            List<int> leaves = findLeavesAmongEdges(remainingEdges, remainingPreviousNodes);
            int i = 0;
            foreach (int leaf in leaves)
            {
                List<int> remainingNodes = remainingPreviousNodes.Select(id => id).ToList();
                remainingNodes.Remove(leaf);
                createSubPossibleKStates(ksWithAllStates, remainingEdges, remainingNodes, someStateProblem, leaf, rnd, leavesCount, i, leaves.Count());
                i++;
            }
        }
        protected List<int> findLeavesAmongEdges(List<List<int>> edges, List<int> nodes)
        {
            List<int> leavesIds = new List<int>();
            foreach (var node in nodes)
            {
                List<List<int>> tempCopy = edges.FindAll(x => x[0] == node);
                if (tempCopy.Count() == 0) 
                {
                    leavesIds.Add(node);
                }
            }
            return leavesIds.Distinct().ToList();
        }
        
        protected async void determinePosibilitiesForAllStates(KnowledgeSpace ksWithAllStates, int realKsId, Dictionary<int, int> reverseMap, HttpClient httpClient)
        {
            int count = 0;
            PossibleStatesWithPossibilities possibleStatesWithPossibilities = new PossibleStatesWithPossibilities();
            foreach (var state in ksWithAllStates.Problems)
            {
                List<int> oneStateList = new List<int>();
                foreach (string oneValueInState in state.Title.Split(' '))
                {
                    int index;
                    if (int.TryParse(oneValueInState, out index))
                    {
                        oneStateList.Add(reverseMap[int.Parse(oneValueInState)]);
                    }
                }
                var json = JsonConvert.SerializeObject(oneStateList);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var result = await httpClient.PostAsync("http://localhost:8000/kst/probability_for_state", content);
                var a = await result.Content.ReadAsStringAsync();

                dynamic numberOfStudentsInState = JsonConvert.DeserializeObject<dynamic>(a);
                possibleStatesWithPossibilities.statePosibilities.Add(state.ProblemId, numberOfStudentsInState);
                count += int.Parse(numberOfStudentsInState);
            }
            foreach (KeyValuePair<int, float> entry in possibleStatesWithPossibilities.statePosibilities)
            {
                possibleStatesWithPossibilities.statePosibilities[entry.Key] = entry.Value / count;
            }
            possibleStatesWithPossibilities.states = ksWithAllStates.Problems;
            possibleStatesWithPossibilities.KnowledgeSpaceId = realKsId;
            possibleStatesWithPossibilities.StudentId = null;
            possibleStatesWithPossibilities.Title =  "Mapping knowlegde states to possibilities for knowledge states: " + ksWithAllStates.KnowledgeSpaceId;
            _possibleStatesWithPossibilitiesRepository.createPossibleStatesWithPossibilities(possibleStatesWithPossibilities);
        }
    }
}
