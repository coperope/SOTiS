using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class KnowledgeSpace
    {
        public int KnowledgeSpaceId { get; set; }
        public int? ProfessorId { get; set; }
        public Professor Professor { get; set; }
        public string Title { get; set; }
        public int? TestId { get; set; }
        public Test Test { get; set; }
        public ICollection<Problem> Problems { get; set; }
        public ICollection<Edge> Edges { get; set; }
    }
}
