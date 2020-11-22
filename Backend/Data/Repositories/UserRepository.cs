using Backend.Data.Context;
using Backend.Data.Repositories.Interfaces;
using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        protected readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<object> GetUserByUsername(string username)
        {
            Student student = await _context.Students.FirstOrDefaultAsync(s => s.Username.Equals(username));
            if (student != null)
            {
                return student;
            }
            Professor professor = await _context.Professors.FirstOrDefaultAsync(s => s.Username.Equals(username));
            if (professor != null)
            {
                return professor;
            }
            return null;
        }

    }
}
