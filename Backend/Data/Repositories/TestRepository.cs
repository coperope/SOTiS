﻿using Backend.Data.Context;
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
    public class TestRepository : ITestRepository
    {
        protected readonly DataContext _context;

        public TestRepository(DataContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<EntityEntry<Test>> MakeTest(Test test)
        {
            EntityEntry<Test> result = await _context.Tests.AddAsync(test);
            _context.SaveChanges();
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public async Task<List<Test>> GetTests()
        {
            List<Test> result = await _context.Tests.Include(t => t.Questions).ThenInclude(q => q.Answers).ToListAsync();
            return result;
        }

        public async Task<int> TakeTest(Test test)
        {
            return 2;
        }
    }
}
