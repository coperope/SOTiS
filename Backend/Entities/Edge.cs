using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Edge
    {
        public int EdgeId { get; set; }
        public int? ProblemSourceId { get; set; }
        public Problem ProblemSource { get; set; }
        public int? ProblemTargetId { get; set; }
        public Problem ProblemTarget { get; set; }
        public int? KnowledgeSpaceId { get; set; }
    }
}
