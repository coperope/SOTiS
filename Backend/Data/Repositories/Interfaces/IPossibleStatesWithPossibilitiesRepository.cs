using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories.Interfaces
{
    public interface IPossibleStatesWithPossibilitiesRepository
    {
        public PossibleStatesWithPossibilities getPossibleStatesWithPossibilities(int knowledgeSpaceId);
        public PossibleStatesWithPossibilities getPossibleStatesWithPossibilitiesForStudent(int student_id);
        public PossibleStatesWithPossibilities createPossibleStatesWithPossibilities(PossibleStatesWithPossibilities possibleStatesWithPossibilities);
        public void updatePossibleStatesWithPossibilities(PossibleStatesWithPossibilities possibleStatesWithPossibilities);
    }
}
