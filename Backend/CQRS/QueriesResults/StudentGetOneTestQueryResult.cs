using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.CQRS.QueriesResults
{
    public class StudentGetOneTestQueryResult : IQueryResult
    {
        public TestView Test { get; set; }

        public class TestView
        {
            public int TestId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public Professor Professor { get; set; }
            public bool Completed { get; set; }
            public ICollection<Question> Questions { get; set; }

        }

        public class Professor
        {
            public int ProfessorId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
        }

        public class Question
        {
            public int QuestionId { get; set; }
            public string Text { get; set; }

            public ICollection<Answer> Answers { get; set; }
            public ICollection<Answer> SelectedAnswers { get; set; }

            public bool IsMultipleChoice { get; set; }
        }

        public class Answer {
            public int AnswerId { get; set; }
            public string Text { get; set; }
            public bool Correct { get; set; }
        }
    }
}
