using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class EnrolementAnswer
    {
        public int EnrolementAnswerId { get; set; }

        public int EnrolementId { get; set; }
        public Enrolement Enrolement { get; set; }

        public int? QuestionId { get; set; }
        public Question Question { get; set; }

        public int? AnswerId { get; set; }
        public Answer Answer { get; set; }

        public bool Skipped { get; set; }
    }
}
