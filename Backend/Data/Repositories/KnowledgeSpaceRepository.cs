using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class KnowledgeSpaceRepository : IKnowledgeSpaceRepository
    {
        protected readonly DataContext _context;

        public KnowledgeSpaceRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<KnowledgeSpace> CreateKnowledgeSpace(KnowledgeSpace knowledgeSpace)
        {
            ICollection<Problem> problems = knowledgeSpace.Problems.Select(s => s).ToList();
            ICollection<Edge> edges = knowledgeSpace.Edges.Select(s => s).ToList();

            knowledgeSpace.Problems = new List<Problem>();
            knowledgeSpace.Edges = new List<Edge>();
            EntityEntry<KnowledgeSpace> result = await _context.KnowledgeSpaces.AddAsync(knowledgeSpace);
            _context.SaveChanges();
            var map = new Dictionary<int, int>();
            foreach (Problem problem in problems) {
                problem.KnowledgeSpaceId = result.Entity.KnowledgeSpaceId;
                int problemID = problem.ProblemId;
                problem.ProblemId = new int();
                var savedProblem = _context.Problems.Add(problem);
                _context.SaveChanges();
                map.Add(problemID, savedProblem.Entity.ProblemId);
            }
            foreach (Edge edge in edges) {
                edge.EdgeId = new int();
                edge.KnowledgeSpaceId = result.Entity.KnowledgeSpaceId;
                edge.ProblemSourceId = map[edge.ProblemSourceId.Value];
                edge.ProblemTargetId = map[edge.ProblemTargetId.Value];
                _context.Edges.Add(edge);
                _context.SaveChanges();
            }
            KnowledgeSpace finalresult = await _context.KnowledgeSpaces.FindAsync(result.Entity.KnowledgeSpaceId);
            if (result != null)
            {
                return finalresult;
            }
            return null;
        }
        public Problem getProblemByTitle(string title)
        {
            return _context.Problems.Where(x => x.Title.Equals(title)).FirstOrDefault();
        }
        public async Task<List<KnowledgeSpace>> GetKnowledgeSpacesOfProfessor(int ProfessorId)
        {
            List<KnowledgeSpace> result = await _context.KnowledgeSpaces
                .Where(t => t.ProfessorId == ProfessorId)
                .Where(t => t.IsReal == false)
                .Include(t => t.Professor)
                .ToListAsync();
            return result;
        }

        public async Task<KnowledgeSpace> GetSingleKnowledgeSpaceByIdWidthIncludes(int id)
        {
            return await _context.KnowledgeSpaces
                .Where(t => t.KnowledgeSpaceId == id)
                .Include(t => t.Professor)
                .Include(t => t.Problems)
                .Include(t => t.Edges)
                    .ThenInclude(q => q.ProblemSource)
                .FirstOrDefaultAsync();
        }

        public async Task<List<KnowledgeSpace>> GetAllRealKSOfOriginalKS(int id)
        {
            return await _context.KnowledgeSpaces
                .Where(t => t.ExpectedKnowledgeSpace == id)
                .Where(t => t.IsReal == true)
                .Include(t => t.Professor)
                .Include(t => t.Problems)
                .Include(t => t.Edges)
                    .ThenInclude(q => q.ProblemSource)
                .ToListAsync();
        }
        
        public async Task<List<KnowledgeSpace>> GetAllPossibleKSOfRealKS(int id)
        {
            return await _context.KnowledgeSpaces
                .Where(t => t.ExpectedKnowledgeSpace == id)
                .Where(t => t.IsReal == false)
                .Include(t => t.Professor)
                .Include(t => t.Problems)
                .Include(t => t.Edges)
                    .ThenInclude(q => q.ProblemSource)
                .ToListAsync();
        }
        public async void updateKnowledgeSpace(KnowledgeSpace knowledgeSpace)
        {
            _context.KnowledgeSpaces.Update(knowledgeSpace);
            _context.SaveChanges();
        }
        public async Task<Problem> addProblem(Problem problem) {
            var retVal = await _context.Problems.AddAsync(problem);
            _context.SaveChanges();
            return retVal.Entity;
        }
        public void updateProblem(Problem problem) { 
            _context.Problems.Update(problem);
            _context.SaveChanges();
        }

        public async Task<Edge> addEdge(Edge edge)
        {
            var retVal = await _context.Edges.AddAsync(edge);
            _context.SaveChanges();
            return retVal.Entity;
        }
    }
}
