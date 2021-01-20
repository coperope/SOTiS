using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        protected readonly DataContext _context;

        public AnswerRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public Answer getAnswerById(int AnswerId)
        {
            return _context.Answers.Find(AnswerId);
        }
    }
}
