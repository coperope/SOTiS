using Backend.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories.Interfaces
{
    public interface IEnrolementRepository
    {
        public Task<Enrolement> CreateEnrolement(Enrolement enrolement);
        public Task<Enrolement> GetByStudentIdAndTestId(int studentId, int testId);
        public Task<Enrolement> GetByStudentIdAndTestIdWithAnswrs(int studentId, int testId);

    }
}
