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
using static Backend.CQRS.QueriesResults.StudentGetOneTestQueryResult;

namespace Backend.CQRS.QueriesHandlers
{
    public class StudentGetNextQuestionQueryHandler : IRequestHandler<StudentGetNextQuestionQuery, StudentGetNextQuestionQueryResult>
    {
        private ITestRepository _testRepository;
        private IEnrolementRepository _enrolementRepository;
        private IEnrolementAnswerRepository _enrolementAnswerRepository;
        private IKnowledgeSpaceRepository _knowledgeSpaceRepository;
        private IPossibleStatesWithPossibilitiesRepository _possibleStatesWithPossibilitiesRepository;
        private IAnswerRepository _answerRepository;
        private IMapper _mapper;
        public StudentGetNextQuestionQueryHandler(ITestRepository testRepository,
            IEnrolementRepository enrolementRepository,
            IKnowledgeSpaceRepository knowledgeSpaceRepository,
            IPossibleStatesWithPossibilitiesRepository possibleStatesWithPossibilitiesRepository,
            IEnrolementAnswerRepository enrolementAnswerRepository,
            IAnswerRepository answerRepository, IMapper mapper)
        {
            _testRepository = testRepository ?? throw new ArgumentNullException(nameof(testRepository));
            _enrolementRepository = enrolementRepository ?? throw new ArgumentNullException(nameof(enrolementRepository));
            _enrolementAnswerRepository = enrolementAnswerRepository ?? throw new ArgumentNullException(nameof(enrolementAnswerRepository));
            _knowledgeSpaceRepository = knowledgeSpaceRepository ?? throw new ArgumentNullException(nameof(knowledgeSpaceRepository));
            _answerRepository = answerRepository ?? throw new ArgumentNullException(nameof(knowledgeSpaceRepository));
            _possibleStatesWithPossibilitiesRepository = possibleStatesWithPossibilitiesRepository ?? throw new ArgumentNullException(nameof(possibleStatesWithPossibilitiesRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<StudentGetNextQuestionQueryResult> Handle(StudentGetNextQuestionQuery request, CancellationToken cancellationToken)
        {
            Test currentTest = await _testRepository.GetSingleTestByIdWithIncludes(request.TestId);
            PossibleStatesWithPossibilities existingpossibleStatesWithPossibilities = _possibleStatesWithPossibilitiesRepository.getPossibleStatesWithPossibilitiesForStudent(request.StudentId);
            Enrolement studentEnrolment = await _enrolementRepository.GetByStudentIdAndTestId(request.StudentId, request.TestId);
            if (request.previousAnsweredQuestion.Answers == null)
            {
                studentEnrolment = new Enrolement();
                studentEnrolment.StudentId = request.StudentId;
                studentEnrolment.TestId = request.TestId;
                studentEnrolment.Completed = false;
                studentEnrolment = await _enrolementRepository.CreateEnrolement(studentEnrolment);

                PossibleStatesWithPossibilities possibleStatesWithPossibilities = _possibleStatesWithPossibilitiesRepository.getPossibleStatesWithPossibilities(currentTest.KnowledgeSpaceId);
                existingpossibleStatesWithPossibilities = new PossibleStatesWithPossibilities();
                //existingpossibleStatesWithPossibilities.states = possibleStatesWithPossibilities.states;
                //foreach (KeyValuePair<int, float> entry in possibleStatesWithPossibilities.statePosibilities)
                //{
                //    existingpossibleStatesWithPossibilities.statePosibilities.Add(entry.Key, entry.Value);
                //}
                existingpossibleStatesWithPossibilities.KnowledgeSpaceId = null;
                existingpossibleStatesWithPossibilities.StudentId = request.StudentId;
                existingpossibleStatesWithPossibilities.Title = "Maintaining probabilities on KS for student: " + request.StudentId;
                existingpossibleStatesWithPossibilities.states = new List<Problem>();
                foreach (Problem problem in possibleStatesWithPossibilities.states)
                {
                    Problem newProblem = new Problem();
                    newProblem.ProblemId = new int();
                    newProblem.KnowledgeSpaceId = problem.KnowledgeSpaceId;
                    newProblem.X = problem.X;
                    newProblem.Y = problem.Y;
                    newProblem.Title = problem.Title;
                    newProblem.statePosibility = problem.statePosibility;
                    var savedProblem = await _knowledgeSpaceRepository.addProblem(newProblem);
                    existingpossibleStatesWithPossibilities.states.Add(savedProblem);
                }
                _possibleStatesWithPossibilitiesRepository.createPossibleStatesWithPossibilities(existingpossibleStatesWithPossibilities);
            } 
            if (request.previousAnsweredQuestion.Answers != null)
            {
                updateProbabilities(existingpossibleStatesWithPossibilities, request.previousAnsweredQuestion);
                saveAnsweredQuestion(request.previousAnsweredQuestion, studentEnrolment.EnrolementId);
                _possibleStatesWithPossibilitiesRepository.updatePossibleStatesWithPossibilities(existingpossibleStatesWithPossibilities);
                
            }
            
            TestView retVal = new TestView();
            retVal = await getNextQuestion(existingpossibleStatesWithPossibilities, request.StudentId, request.TestId, currentTest);
            return new StudentGetNextQuestionQueryResult()
            {
                TestToReturn = retVal
            };
        }

        protected void updateProbabilities(PossibleStatesWithPossibilities existingpossibleStatesWithPossibilities, StudentGetOneTestQueryResult.Question answeredQuestion)
        {
            List<int> realAnswersCorrect = new List<int>();
            foreach (StudentGetOneTestQueryResult.Answer a in answeredQuestion.Answers)
            {
                Entities.Answer realAnswer = _answerRepository.getAnswerById(a.AnswerId);
                if (realAnswer.Correct)
                {
                    realAnswersCorrect.Add(realAnswer.AnswerId);
                }
            }
            int flag = 0;
            foreach (StudentGetOneTestQueryResult.Answer studentsAnswer in answeredQuestion.SelectedAnswers)
            {
                if (realAnswersCorrect.Contains(studentsAnswer.AnswerId))
                {
                    flag += 1;
                }
            }
            if (realAnswersCorrect.Count() == flag)
            {
                foreach (Problem state in existingpossibleStatesWithPossibilities.states)
                {
                    List<string> splitedTitle = state.Title.Split(" ").ToList();
                    if (splitedTitle.Contains(answeredQuestion.ProblemId.ToString()))
                    {
                       state.statePosibility = (float)(state.statePosibility * 1.3);
                    }
                }
            }
            else
            {
                foreach (Problem state in existingpossibleStatesWithPossibilities.states)
                {
                    List<string> splitedTitle = state.Title.Split(" ").ToList();
                    if (splitedTitle.Contains(answeredQuestion.ProblemId.ToString()))
                    {
                        state.statePosibility = (float)(state.statePosibility * 0.6);
                    }
                }
            }
        }
        protected async Task<TestView> getNextQuestion(PossibleStatesWithPossibilities existingpossibleStatesWithPossibilities, int student_id, int test_id, Test test)
        {
            Enrolement enrolement = await _enrolementRepository.GetByStudentIdAndTestId(student_id, test_id);
            Dictionary<int, float> questionPossibility = new Dictionary<int, float>();
            List<Entities.Question> forItteration = enrolement.EnrolementAnswers != null ?
                test.Questions.Where(x => enrolement.EnrolementAnswers.Where(t => t.QuestionId.Equals(x.QuestionId)).Count() == 0).ToList() 
                :
                test.Questions.ToList();
            foreach (Entities.Question q in forItteration)
            {
                foreach (Problem problem in existingpossibleStatesWithPossibilities.states)
                {
                    List<string> splitedTitle = problem.Title.Split(" ").ToList();
                    if (splitedTitle.Contains(q.ProblemId.ToString()))
                    {
                        float temp = questionPossibility.ContainsKey(q.QuestionId) ? questionPossibility[q.QuestionId] : 0;
                        questionPossibility[q.QuestionId] = (float)(temp + problem.statePosibility);
                    }
                }
            }

            if (forItteration.Count != 0)
            {
                int maxKeyQuestion = questionPossibility.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
                TestView retVal = new TestView();
                _mapper.Map(test, retVal);
                retVal.Questions = new List<StudentGetOneTestQueryResult.Question>();
                retVal.Questions.Add(_mapper.Map<Entities.Question, StudentGetOneTestQueryResult.Question>(forItteration.Find(x => x.QuestionId == maxKeyQuestion)));
                foreach (StudentGetOneTestQueryResult.Question q in retVal.Questions)
                {
                    q.SelectedAnswers = new List<StudentGetOneTestQueryResult.Answer>();
                }

                return retVal;
            } else
            {
                enrolement.Completed = true;
                _enrolementRepository.UpdateEnrolement(enrolement);
                TestView retVal = new TestView();
                _mapper.Map(test, retVal);
                retVal.Questions = new List<StudentGetOneTestQueryResult.Question>();
                foreach (StudentGetOneTestQueryResult.Question q in retVal.Questions)
                {
                    q.SelectedAnswers = new List<StudentGetOneTestQueryResult.Answer>();
                }
                return retVal;
            }
        }
        protected async void saveAnsweredQuestion(StudentGetOneTestQueryResult.Question q, int enrolementId)
        {
            if (q.SelectedAnswers?.Count > 0)
            {
                foreach (StudentGetOneTestQueryResult.Answer a in q.SelectedAnswers)
                {
                    EnrolementAnswer enrAnsw = new EnrolementAnswer();
                    enrAnsw.EnrolementId = enrolementId;
                    enrAnsw.QuestionId = q.QuestionId;
                    enrAnsw.AnswerId = a.AnswerId;
                    await _enrolementAnswerRepository.CreateEnrolementAnswer(enrAnsw);
                }
            }
            else
            {
                EnrolementAnswer enrAnsw = new EnrolementAnswer();
                enrAnsw.EnrolementId = enrolementId;
                enrAnsw.QuestionId = q.QuestionId;
                enrAnsw.Skipped = true;
                await _enrolementAnswerRepository.CreateEnrolementAnswer(enrAnsw);
            }
        }
    }
}
