using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;

namespace Backend.CQRS.Queries
{
    public class GetTestQtiQuery : IQuery, IRequest<GetTestQtiQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.All;
        public int? UserId { get; set; }
        public int TestId { get; set; }
    }
}
