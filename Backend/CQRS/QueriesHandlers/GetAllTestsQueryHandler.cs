using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Data.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesHandlers
{
    public class GetAllTestsQueryHandler : IRequestHandler<GetAllTestsQuery, GetAllTestsQueryResult>
    {
        private ITestRepository _testRepository;
        public GetAllTestsQueryHandler(ITestRepository testRepository)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
        }
        public async Task<GetAllTestsQueryResult> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
        {
            var result = _testRepository.GetTests();

            return new GetAllTestsQueryResult
            {
                Tests = result
            };

        }
    }
}
