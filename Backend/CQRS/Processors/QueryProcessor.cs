using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Utils.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public class QueryProcessor : IQueryProcessor
    {
        private readonly IMediator _mediator;
        private IHttpContextAccessor _httpContext;

        public QueryProcessor(IMediator mediator, IHttpContextAccessor context)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _httpContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IQueryResult> Execute(IQuery query)
        {

            if (!hasValidPermission(query.Permission))
            {
                throw new Exception("Not GUD");
            }
            query.UserId = setUserId();


            var result = await _mediator.Send(query);

            return result as IQueryResult;
        }
        private bool hasValidPermission(CQRSRole permission)
        {
            return permission == CQRSRole.All || (_httpContext.HttpContext.Items["UserRole"] != null && (CQRSRole)_httpContext.HttpContext.Items["UserRole"] == permission);
        }

        private int? setUserId()
        {
            if (_httpContext.HttpContext.Items["UserId"] != null)
            {
                return (int)_httpContext.HttpContext.Items["UserId"];
            }
            return null;
        }
    }
}
