using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Professor> Professors { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Enrolement> Enrolements { get; set; }
        public DbSet<EnrolementAnswer> EnrolementAnswers { get; set; }
        public DbSet<KnowledgeSpace> KnowledgeSpaces { get; set; }
        public DbSet<Edge> Edges { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<PossibleStatesWithPossibilities> PossibleStatesWithPossibilities { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

    }
}
