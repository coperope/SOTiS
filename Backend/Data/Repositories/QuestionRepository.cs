using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        protected readonly DataContext _context;

        public QuestionRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Question>> GetQuestionsByTestId(int testId)
        {
            return await _context.Questions.Where(q => q.TestId == testId).ToListAsync();
        }
    }
}
