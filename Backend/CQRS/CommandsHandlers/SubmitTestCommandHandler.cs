using AutoMapper;
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
    public class SubmitTestCommandHandler : IRequestHandler<SubmitTestCommand, SubmitTestCommandResult>
    {
        private IEnrolementRepository _enrolementRepository;
        private IEnrolementAnswerRepository _enrolementAnswerRepository;
        private IQuestionRepository _questionRepository;

        private IMapper _mapper;

        public SubmitTestCommandHandler(IEnrolementRepository enrolementRepository, IEnrolementAnswerRepository enrolementAnswerRepository, IQuestionRepository qquestionRepository, IMapper mapper)
        {
            _enrolementRepository = enrolementRepository ?? throw new ArgumentNullException(nameof(enrolementRepository));
            _enrolementAnswerRepository = enrolementAnswerRepository ?? throw new ArgumentNullException(nameof(enrolementAnswerRepository));
            _questionRepository = qquestionRepository ?? throw new ArgumentNullException(nameof(qquestionRepository));

            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<SubmitTestCommandResult> Handle(SubmitTestCommand request, CancellationToken cancellationToken)
        {
            Enrolement enr = new Enrolement();
            enr.StudentId = request.UserId.Value;
            enr.TestId = request.Test.TestId;
            enr.Completed = true;
            Enrolement enrolement = await _enrolementRepository.CreateEnrolement(enr);

            
            foreach (SubmitTestQuestion q in request.Test.Questions)
            {
                if (q.SelectedAnswers?.Count > 0)
                {
                    foreach (SubmitTestAnswer a in q.SelectedAnswers)
                    {
                        EnrolementAnswer enrAnsw = new EnrolementAnswer();
                        enrAnsw.EnrolementId = enrolement.EnrolementId;
                        enrAnsw.QuestionId = q.QuestionId;
                        enrAnsw.AnswerId = a.AnswerId;
                        await _enrolementAnswerRepository.CreateEnrolementAnswer(enrAnsw);
                    }
                } else
                {
                    EnrolementAnswer enrAnsw = new EnrolementAnswer();
                    enrAnsw.EnrolementId = enrolement.EnrolementId;
                    enrAnsw.QuestionId = q.QuestionId;
                    enrAnsw.Skipped = true;
                    await _enrolementAnswerRepository.CreateEnrolementAnswer(enrAnsw);
                }
            }

            return new SubmitTestCommandResult
            {
                EnrolementId = enrolement.EnrolementId,
                Success = true,
            };
        }
    }
}
