using AutoMapper;
using Backend.CQRS.Queries;
using Backend.CQRS.QueriesResults;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesHandlers
{
    public class StudentGetAllTestsQueryHandler : IRequestHandler<StudentGetAllTestsQuery, StudentGetAllTestsQueryResult>
    {
        private ITestRepository _testRepository;
        private IEnrolementRepository _enrolementRepository;

        private IMapper _mapper;

        public StudentGetAllTestsQueryHandler(ITestRepository testRepository, IEnrolementRepository enrolementRepository, IMapper mapper)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            _enrolementRepository = enrolementRepository ?? throw new ArgumentNullException(nameof(enrolementRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<StudentGetAllTestsQueryResult> Handle(StudentGetAllTestsQuery request, CancellationToken cancellationToken)
        {
            var result = await _testRepository.GetTestsWithProfessor();
            List<StudentGetAllTestsQueryResult.Test> destination = new List<StudentGetAllTestsQueryResult.Test>();
            foreach (Test res in result)
            {
                StudentGetAllTestsQueryResult.Test testview = new StudentGetAllTestsQueryResult.Test();
                _mapper.Map(res, testview);

                if (await _enrolementRepository.GetByStudentIdAndTestId(request.UserId.Value, res.TestId) != null) {
                    // TODO: Change when (if) we add temporary save
                    testview.Completed = true;
                }

                destination.Add(testview);
            }

            return new StudentGetAllTestsQueryResult
            {
                Tests = destination
            };

        }
    }
}
