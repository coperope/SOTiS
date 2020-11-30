using Backend.CQRS.Commands;
using Backend.CQRS.Processors;
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
    public class ProfessorController : ControllerBase
    {
        private readonly ICommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IHttpContextAccessor _httpContext;

        public ProfessorController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor, IHttpContextAccessor context)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
            _httpContext = context;
        }

        [HttpPost("{professor_id}/tests")]
        public async Task<IActionResult> MakeTest(String professor_id, MakeTestCommand makeTestCommand)
        {
            makeTestCommand.ProfessorId = int.Parse(professor_id);
            var response = await _commandProcessor.Execute(makeTestCommand, _httpContext);
            return Ok();
        }

        [HttpGet("{professor_id}/tests")]
        public async Task<IActionResult> GetAllProfessorsTests(String professor_id)
        {
            return Ok();
        }

        [HttpGet("{professor_id}/tests/{test_id}")]
        public async Task<IActionResult> GetOneProfessorsTest(String professor_id, String test_id)
        {
            System.Diagnostics.Debug.WriteLine("Professor id: " + professor_id);
            System.Diagnostics.Debug.WriteLine("Testt id: " + test_id);

            return Ok();
        }

        [HttpDelete("{professor_id}/tests/{test_id}")]
        public async Task<IActionResult> DeleteOneProfessorsTest(String professor_id, String test_id)
        {
            System.Diagnostics.Debug.WriteLine("Professor id: " + professor_id);
            System.Diagnostics.Debug.WriteLine("Testt id: " + test_id);

            return Ok();
        }
    }
}
