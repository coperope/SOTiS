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
    public class StudentGetOneTestQueryHandler : IRequestHandler<StudentGetOneTestQuery, StudentGetOneTestQueryResult>
    {
        private ITestRepository _testRepository;
        private IEnrolementRepository _enrolementRepository;

        private IMapper _mapper;

        public StudentGetOneTestQueryHandler(ITestRepository testRepository, IEnrolementRepository enrolementRepository, IMapper mapper)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            _enrolementRepository = enrolementRepository ?? throw new ArgumentNullException(nameof(enrolementRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<StudentGetOneTestQueryResult> Handle(StudentGetOneTestQuery request, CancellationToken cancellationToken)
        {
            var test = await _testRepository.GetSingleTestByIdWithIncludes(request.TestId);

            var testView = new StudentGetOneTestQueryResult.TestView();
            _mapper.Map(test, testView);

            // TODO: Change when (if) we add temporary save
            if (await _enrolementRepository.GetByStudentIdAndTestId(request.UserId.Value, test.TestId) != null)
            {
                testView.Completed = true;
                Enrolement enrolement = await _enrolementRepository.GetByStudentIdAndTestIdWithAnswrs(request.UserId.Value, request.TestId);

                foreach (EnrolementAnswer studentAnswer in enrolement.EnrolementAnswers)
                {
                    System.Diagnostics.Debug.WriteLine(studentAnswer.AnswerId);
                    var question = testView.Questions
                        .Where(q => q.QuestionId == studentAnswer.QuestionId)
                        .FirstOrDefault();
                    if (question.SelectedAnswers == null)
                    {
                        question.SelectedAnswers = new List<StudentGetOneTestQueryResult.Answer>();
                    }
                    if (!studentAnswer.Skipped)
                    {
                        question.SelectedAnswers.Add(_mapper.Map(studentAnswer.Answer, new StudentGetOneTestQueryResult.Answer()));
                        question.SelectedAnswers.ToList().ForEach(a => a.Text = "");
                    }
                }
            } else
            {
                testView.Questions
                    .ToList()
                    .ForEach(q => 
                    { 
                        q.SelectedAnswers = new List<StudentGetOneTestQueryResult.Answer>();
                        q.Answers.ToList().ForEach(a => a.Correct = false);
                    });
            }

            return new StudentGetOneTestQueryResult
            {
                Test = testView
            };

        }
    }
}
