﻿using Backend.Entities;
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
        public Task<List<KnowledgeSpace>> GetRealKnowledgeSpacesOfProfessor(int ProfessorId);
        public Task<KnowledgeSpace> GetSingleKnowledgeSpaceByIdWidthIncludes(int id);
        public Task<List<KnowledgeSpace>> GetAllRealKSOfOriginalKS(int id);
        public  void updateKnowledgeSpace(KnowledgeSpace knowledgeSpace);
        public Task<Problem> addProblem(Problem problem);
        public void updateProblem(Problem problem);
        public Task<Edge> addEdge(Edge edge);
        public Problem getProblemByTitle(string title);
        public Task<List<KnowledgeSpace>> GetAllPossibleKSOfRealKS(int id);
    }
}
