using Backend.CQRS.Commands;
using Backend.CQRS.Processors;
using Backend.CQRS.Queries;
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


        public StudentController(ICommandProcessor commandProcessor, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _queryProcessor = queryProcessor;
        }

        [HttpPost("{student_id}/test/{test_id}")]
        public async Task<IActionResult> TakeTest(String student_id, String test_id, TakeTestCommand takeTestCommand)
        {
            var result = await _commandProcessor.Execute(takeTestCommand);
            return Ok();
        }

        [HttpGet("tests")]
        public async Task<IActionResult> GetAllTests()
        {
            var result = await _queryProcessor.Execute(new GetAllTestsQuery());
            return Ok(result);
        }

        [HttpGet("{student_id}/test/{test_id}")]
        public async Task<IActionResult> GetOneTest(String student_id, String test_id)
        {
            System.Diagnostics.Debug.WriteLine("Student id: " + student_id);
            System.Diagnostics.Debug.WriteLine("Testt id: " + test_id);

            return Ok();
        }
    }
}
