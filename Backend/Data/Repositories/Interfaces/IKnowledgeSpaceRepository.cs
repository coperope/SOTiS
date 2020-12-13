using Backend.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Repositories.Interfaces
{
    public interface IKnowledgeSpaceRepository
    {
        public Task<KnowledgeSpace> CreateKnowledgeSpace(KnowledgeSpace knowledgeSpace);
        public Task<List<KnowledgeSpace>> GetKnowledgeSpacesOfProfessor(int ProfessorId);
        public Task<KnowledgeSpace> GetSingleKnowledgeSpaceByIdWidthIncludes(int id);
        public Task<List<KnowledgeSpace>> GetAllRealKSOfOriginalKS(int id);
    }
}
