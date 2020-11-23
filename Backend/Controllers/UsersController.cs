using Backend.CQRS.Commands;
using Backend.CQRS.Processors;
using Backend.CQRS.Queries;
using Backend.Utils.CustomAttributes;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;

        public UsersController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate(AuthenticateQuery query)
        {
            var response = await _queryProcessor.Execute(query);

            if (response == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command)
        {
            var response = await _commandProcessor.Execute(command);

            if (response == null)
                return BadRequest(new { message = "You stupid moron." });

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = "STIGAO";
            return Ok(users);
        }
    }
}
