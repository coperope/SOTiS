using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class StudentCurrentTestRepository : IStudentCurrentTestRepository
    {
        protected readonly DataContext _context;

        public StudentCurrentTestRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public  StudentCurrentTest getStudentCurrentTest(int student_id, int test_id)
        {
            var retVal = _context.StudentCurrentTests.Where(x => x.StudentId.Equals(student_id)).Where(y => y.TestId.Equals(test_id)).FirstOrDefault();
            return retVal;
        }
        public async Task<StudentCurrentTest> startStudentCurrentTest(int student_id, int test_id, Test test, int enrolementId, List<int> QuestionsLeft)
        {
            StudentCurrentTest studentCurrentTest = new StudentCurrentTest();
            studentCurrentTest.StudentId = student_id;
            studentCurrentTest.TestId = test_id;
            studentCurrentTest.finished = false;
            studentCurrentTest.Test = test;
            studentCurrentTest.QuestionsLeft = QuestionsLeft;
            studentCurrentTest.studentsEnrolmentId = enrolementId;
            var retVal = await _context.StudentCurrentTests.AddAsync(studentCurrentTest);
            return retVal.Entity;
        }
        public void updateStudentCurrentTest(StudentCurrentTest studentCurrentTests)
        {
            _context.StudentCurrentTests.Update(studentCurrentTests);
            _context.SaveChanges();
        }
    }
}
