using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories.Interfaces
{
    public interface IStudentCurrentTestRepository
    {
        public StudentCurrentTest getStudentCurrentTest(int student_id, int test_id);
        public Task<StudentCurrentTest> startStudentCurrentTest(int student_id, int test_id, Test test, int enrolementId, List<int> QuestionsLeft);
        public void updateStudentCurrentTest(StudentCurrentTest studentCurrentTest);
    }
}
