using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }

        public int TestId { get; set; }
        public Test Test { get; set; }

        public ICollection<Answer> Answers { get; set; }
        public bool IsMultipleChoice { get; set; }
    }
}
