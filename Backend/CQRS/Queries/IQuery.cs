using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;

namespace Backend.CQRS.Queries
{
    public interface IQuery
    {
        public CQRSRole Permission { get; }
        public int? UserId { get; set; }

    }
}
