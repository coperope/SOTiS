using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IMediator _mediator;

        public QueryProcessor(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IQueryResult> Execute(IQuery query)
        {

            // check if query accepts that role, else throw error


            var result = await _mediator.Send(query);

            return result as IQueryResult;
        }
    }
}
