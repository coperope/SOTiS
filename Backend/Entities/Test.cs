using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Test
    {
        public int TestId { get; set; }
        public string Title { get; set; }

        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }
        public string Description { get; set; }
        public int KnowledgeSpaceId { get; set; }
        public KnowledgeSpace KnowledgeSpace { get; set; }
        public ICollection<Question> Questions { get; set; }

        public ICollection<Enrolement> Enrolements { get; set; }
    }
}
