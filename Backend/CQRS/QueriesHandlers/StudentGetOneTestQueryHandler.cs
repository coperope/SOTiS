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
        private IKnowledgeSpaceRepository _knowledgeSpaceRepository;

        private IMapper _mapper;

        public StudentGetOneTestQueryHandler(ITestRepository testRepository, IEnrolementRepository enrolementRepository, IKnowledgeSpaceRepository knowledgeSpaceRepository, IMapper mapper)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            _enrolementRepository = enrolementRepository ?? throw new ArgumentNullException(nameof(enrolementRepository));
            _knowledgeSpaceRepository = knowledgeSpaceRepository ?? throw new ArgumentNullException(nameof(knowledgeSpaceRepository));

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

            var sorted = await SortQuestions(testView);
            var isolated = testView.Questions.Where(q => !sorted.Contains(q)).ToList();
            sorted.AddRange(isolated);

            testView.Questions = sorted;

            return new StudentGetOneTestQueryResult
            {
                Test = testView
            };

        }

        public async Task<List<StudentGetOneTestQueryResult.Question>> SortQuestions(StudentGetOneTestQueryResult.TestView test)
        {
            var knowledgeSpace = await _knowledgeSpaceRepository.GetSingleKnowledgeSpaceByIdWidthIncludes(test.KnowledgeSpaceId);
            var edges = knowledgeSpace.Edges.ToList();
            var sortedQuestions = new List<StudentGetOneTestQueryResult.Question>();

            var leaves = edges.Where(edge => !edges.Any(e => e.ProblemSourceId == edge.ProblemTargetId)).Select(e => e.ProblemTargetId).Distinct().ToList();

            TravereseGraph(leaves, edges, sortedQuestions, test);

            return sortedQuestions;
        }

        public void TravereseGraph(List<int?> nodes, List<Edge> edges, List<StudentGetOneTestQueryResult.Question> questions, StudentGetOneTestQueryResult.TestView test)
        {
            foreach (var node in nodes)
            {
                var parents = edges.Where(e => e.ProblemTargetId == node).Select(e => e.ProblemSourceId).ToList();
                if (!parents.Any())
                {
                    questions.Add(test.Questions.First(q => q.ProblemId == node));
                    edges.RemoveAll(e => e.ProblemSourceId == node);
                    continue;
                }

                TravereseGraph(parents, edges, questions, test);

                questions.Add(test.Questions.First(q => q.ProblemId == node));
                edges.RemoveAll(e => e.ProblemSourceId == node);
            }
        }
    }
}
