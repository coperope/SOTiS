using System.Collections.Generic;

namespace Backend.CQRS.QueriesResults
{
    public class StudentGetAllTestsQueryResult : IQueryResult
    {
        public List<Test> Tests { get; set; }

        public class Test
        {
            public int TestId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Professor Professor { get; set; }
            public bool Completed { get; set; }
            public int? KnowledgeSpaceId { get; set; }
        }

        public class Professor
        {
            public int ProfessorId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }
    }

}
