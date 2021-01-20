using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Entities
{
    public class StudentCurrentTest
    {
        public int StudentId { get; set; }
        public string Title { get; set; }

        [ForeignKey("Test")]
        public int? TestId { get; set; }
        public Test Test { get; set; }
        [ForeignKey("Enrolment")]
        public int studentsEnrolmentId { get; set; }
        public Enrolement studentsEnrolment { get; set; }
        public List<int> QuestionsLeft { get; set; } 
        public bool finished { get; set; }

        
    }
}
