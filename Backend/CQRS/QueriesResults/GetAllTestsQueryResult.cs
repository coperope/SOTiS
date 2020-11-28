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
        public string Description { get; set; }
        public ProfessorTestView Professor { get; set; }
    }

    public class ProfessorTestView
    {
        public int ProfessorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
