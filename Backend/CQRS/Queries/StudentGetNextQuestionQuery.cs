using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Backend.CQRS.QueriesResults.StudentGetOneTestQueryResult;

namespace Backend.CQRS.Queries
{
    public class StudentGetNextQuestionQuery : IQuery, IRequest<StudentGetNextQuestionQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Student;
        public int? UserId { get; set; }
        public int TestId { get; set; }
        public int StudentId { get; set; }
        public Question previousAnsweredQuestion { get; set; }
    }
}
