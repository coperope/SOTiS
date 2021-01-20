using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class PossibleStatesWithPossibilities
    {
        public int PossibleStatesWithPossibilitiesId { get; set; }
        public int? KnowledgeSpaceId { get; set; }
        public string Title { get; set; }
        public int? StudentId { get; set; }
        public ICollection<Problem> states { get; set; }
        [NotMapped]
        public Dictionary<int, float> statePosibilities = new Dictionary<int, float>();
    }
}
