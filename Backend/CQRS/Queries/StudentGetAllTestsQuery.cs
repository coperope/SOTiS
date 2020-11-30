using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;

namespace Backend.CQRS.Queries
{
    public class StudentGetAllTestsQuery : IQuery, IRequest<StudentGetAllTestsQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Student;
        public int? UserId { get; set; }

    }
}
