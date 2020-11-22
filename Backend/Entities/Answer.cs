using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public string Text { get; set; }
        public bool Correct { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
