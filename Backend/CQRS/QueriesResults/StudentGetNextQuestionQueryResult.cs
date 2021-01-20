using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Backend.CQRS.QueriesResults.StudentGetOneTestQueryResult;

namespace Backend.CQRS.QueriesResults
{
    public class StudentGetNextQuestionQueryResult : IQueryResult
    {
        public TestView TestToReturn { get; set; }
    }
}
