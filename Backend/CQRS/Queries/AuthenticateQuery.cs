using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Backend.CQRS.Queries
{
    public class AuthenticateQuery : IQuery, IRequest<AuthenticateQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.All;
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
