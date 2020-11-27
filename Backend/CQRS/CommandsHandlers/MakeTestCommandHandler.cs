using Backend.CQRS.Commands;
using Backend.CQRS.CommandsResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.CommandsHandlers
{
    public class MakeTestCommandHandler : IRequestHandler<MakeTestCommand, MakeTestCommandResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITestRepository _testRepository;

        public MakeTestCommandHandler(IUserRepository userRepository, ITestRepository testRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        }
        public async Task<MakeTestCommandResult> Handle(MakeTestCommand request, CancellationToken cancellationToken)
        {
            Test test = new Test
            {
                Title = request.Title,
                Description = request.Description,
                ProfessorId = request.ProfessorId,
                Questions = request.Questions
            };
            var result = await _testRepository.MakeTest(test);
            return new MakeTestCommandResult
            {
                Id = result.Entity.TestId
            };
        }
    }
}
