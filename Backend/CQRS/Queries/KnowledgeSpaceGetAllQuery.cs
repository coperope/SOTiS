using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;

namespace Backend.CQRS.Queries
{
    public class KnowledgeSpaceGetAllQuery : IQuery, IRequest<KnowledgeSpaceGetAllQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Professor;
        public int? UserId { get; set; }
        public int ProfessorId { get; set; }
    }
}
