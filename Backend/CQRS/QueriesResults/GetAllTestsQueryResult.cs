using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesResults
{
    public class GetAllTestsQueryResult : IQueryResult
    {
        public List<TestView> Tests { get; set; }
    }
    public class TestView
    {
        public int TestId { get; set; }
        public string Title { get; set; }

        public int ProfessorId { get; set; }
        public Professor Professor { get; set; }
        public string Description { get; set; }
    }
}
