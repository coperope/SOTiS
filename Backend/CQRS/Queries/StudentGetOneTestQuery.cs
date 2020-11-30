using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;

namespace Backend.CQRS.Queries
{
    public class StudentGetOneTestQuery : IQuery, IRequest<StudentGetOneTestQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Student;
        public int? UserId { get; set; }
        public int TestId { get; set; }

    }
}
