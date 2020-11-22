using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Data.Context
{
    public class DataContextSeed
    {
        public static async Task SeedAsync(DataContext dataContext, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            try
            {
                dataContext.Database.Migrate();

                if (!dataContext.Students.Any())
                {
                    dataContext.Students.AddRange(GetPreconfiguredStudents());
                    await dataContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                if (retryForAvailability < 3)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<DataContextSeed>();
                    log.LogError(exception.Message);
                    await SeedAsync(dataContext, loggerFactory, retryForAvailability);
                }
            }
        }

        private static IEnumerable<Student> GetPreconfiguredStudents()
        {
            return new List<Student>
            {
                new Student() {Username = "test", FirstName = "Ime", LastName = "Prezime", Password = "test"  }
            };
        }
    }
}
