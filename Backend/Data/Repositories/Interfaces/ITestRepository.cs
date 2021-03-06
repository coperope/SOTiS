﻿using Backend.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories.Interfaces
{
    public interface ITestRepository
    {
        public Task<EntityEntry<Test>> MakeTest(Test test);
        public Task<List<Test>> GetTestsWithProfessor();
        public Task<Test> GetSingleTestById(int id);
        public Task<Test> GetSingleTestByIdWithIncludes(int id);

    }
}
