using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class PossibleStatesWithPossibilitiesRepository : IPossibleStatesWithPossibilitiesRepository
    {
        protected readonly DataContext _context;

        public PossibleStatesWithPossibilitiesRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void createPossibleStatesWithPossibilities(PossibleStatesWithPossibilities possibleStatesWithPossibilities)
        {
            _context.PossibleStatesWithPossibilities.Add(possibleStatesWithPossibilities);
            _context.SaveChanges();
        }

        public PossibleStatesWithPossibilities getPossibleStatesWithPossibilities(int knowledgeSpaceId)
        {
            return _context.PossibleStatesWithPossibilities
                .Where(x => x.KnowledgeSpaceId == knowledgeSpaceId)
                .Include(x => x.states)
                .FirstOrDefault();
        }
        public PossibleStatesWithPossibilities getPossibleStatesWithPossibilitiesForStudent(int student_id)
        {
            return _context.PossibleStatesWithPossibilities
                .Where(x => x.StudentId == student_id)
                .Include(x => x.states)
                .FirstOrDefault();
        }

        public void updatePossibleStatesWithPossibilities(PossibleStatesWithPossibilities possibleStatesWithPossibilities)
        {
            _context.PossibleStatesWithPossibilities.Update(possibleStatesWithPossibilities);
            _context.SaveChanges();
        }
    }
}
