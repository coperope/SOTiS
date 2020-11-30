using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Backend.Utils.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Processors
{
    public class CommandProcessor : ICommandProcessor
    {
        private readonly IMediator _mediator;
        private IHttpContextAccessor _httpContext;

        public CommandProcessor(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<ICommandResult> Execute(ICommand command, IHttpContextAccessor context)
        {
            _httpContext = context;

            if (!hasValidPermission(command.Permission))
            {
                throw new Exception("Not GUD");
            }
            command.UserId = setUserId();

            var result = await _mediator.Send(command);

            return result as ICommandResult;
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
