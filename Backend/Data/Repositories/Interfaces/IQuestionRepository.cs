using Backend.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestionsByTestId(int testId);
    }
}
