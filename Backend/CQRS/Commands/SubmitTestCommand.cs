using Backend.CQRS.CommandsResults;
using Backend.Entities;
using Backend.Utils.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.Commands
{
    public class SubmitTestCommand : ICommand, IRequest<SubmitTestCommandResult>
    {
        public CQRSRole Permission { get; } = CQRSRole.Student;
        public int? UserId { get; set; }
        public SubmitTestTest Test { get; set; }
    }

    public class SubmitTestTest
    {
        public int TestId { get; set; }

        public ICollection<SubmitTestQuestion> Questions { get; set; }
    }

    public class SubmitTestQuestion
    {
        public int QuestionId { get; set; }

        public ICollection<SubmitTestAnswer> SelectedAnswers { get; set; }
        public bool IsMultipleChoice { get; set; }
    }

    public class SubmitTestAnswer
    {
        public int AnswerId { get; set; }
    }
}
