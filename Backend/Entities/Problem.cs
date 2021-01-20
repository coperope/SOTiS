using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Problem
    {
        public int ProblemId { get; set; }
        public string Title { get; set; }
        public int KnowledgeSpaceId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double statePosibility { get; set; }
    }
}
