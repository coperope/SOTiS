using Backend.CQRS.Commands;
using Backend.CQRS.Processors;
using Backend.CQRS.Queries;
using Backend.Utils.CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IHttpContextAccessor _httpContext;

        public StudentController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor, IHttpContextAccessor context)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
            _httpContext = context;
        }

        [Authorize]
        [HttpPost("{student_id}/test/{test_id}")]
        public async Task<IActionResult> SubmitTest(String student_id, String test_id, SubmitTestCommand submitTestCommand)
        {
            var result = await _commandProcessor.Execute(submitTestCommand, _httpContext);
            return Ok();
        }

        [Authorize]
        [HttpGet("tests")]
        public async Task<IActionResult> GetAllTests()
        {
            var result = await _queryProcessor.Execute(new StudentGetAllTestsQuery(), _httpContext);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("{student_id}/test/{test_id}")]
        public async Task<IActionResult> GetOneTest(String student_id, int test_id)
        {
            var result = await _queryProcessor.Execute(new StudentGetOneTestQuery() { TestId = test_id }, _httpContext);
            return Ok(result);
        }
    }
}
