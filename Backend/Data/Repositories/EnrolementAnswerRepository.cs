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
    public class EnrolementAnswerRepository : IEnrolementAnswerRepository
    {
        protected readonly DataContext _context;

        public EnrolementAnswerRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<EnrolementAnswer> CreateEnrolementAnswer(EnrolementAnswer enrolementAnswer)
        {
            EntityEntry<EnrolementAnswer> result = await _context.EnrolementAnswers.AddAsync(enrolementAnswer);
            _context.SaveChanges();
            if (result != null)
            {
                return result.Entity;
            }
            return null;
        }
    }
}
