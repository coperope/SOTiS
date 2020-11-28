using AutoMapper;
using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
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
        private IMapper _mapper;
        public GetAllTestsQueryHandler(ITestRepository testRepository, IMapper mapper)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<GetAllTestsQueryResult> Handle(GetAllTestsQuery request, CancellationToken cancellationToken)
        {
            var result = await _testRepository.GetTests();
            List<TestView> destination = new List<TestView>();
            foreach (Test res in result)
            {
                TestView testview = new TestView();
                _mapper.Map(res, testview);
                destination.Add(testview);
            }

            return new GetAllTestsQueryResult
            {
                Tests = destination
            };

        }
    }
}
