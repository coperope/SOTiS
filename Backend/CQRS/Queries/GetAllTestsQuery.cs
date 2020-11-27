using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Queries
{
    public class GetAllTestsQuery : IQuery, IRequest<GetAllTestsQueryResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.All;
    }
}
