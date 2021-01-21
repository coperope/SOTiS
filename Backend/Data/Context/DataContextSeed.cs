using Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                ClearDatabase(dataContext);
                dataContext.Database.Migrate();

                dataContext.Students.AddRange(GetPreconfiguredStudents());
                dataContext.Professors.AddRange(GetPreconfiguredProfessors());
                await dataContext.SaveChangesAsync();
                dataContext.KnowledgeSpaces.AddRange(GetPreconfiguredKnowledgeSpaces());
                await dataContext.SaveChangesAsync();
                dataContext.Problems.AddRange(GetPreconfiguredProblems());
                await dataContext.SaveChangesAsync();
                dataContext.Edges.AddRange(GetPreconfiguredEdges());
                await dataContext.SaveChangesAsync();
                dataContext.Tests.AddRange(GetPreconfiguredTests());
                await dataContext.SaveChangesAsync();
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
                new Student() {Username = "student", FirstName = "Vladimir", LastName = "Iljic Lenjin", Password = "123456"  }
            };
        }
        private static IEnumerable<Professor> GetPreconfiguredProfessors()
        {
            return new List<Professor>
            {
                new Professor() {Username = "professor", FirstName = "Karl", LastName = "Marks", Password = "123456"  }
            };
        }
        private static IEnumerable<KnowledgeSpace> GetPreconfiguredKnowledgeSpaces()
        {
            return new List<KnowledgeSpace>
            {
                new KnowledgeSpace() {ProfessorId = 1, Title = "ks1", IsReal = false},
            };
        }

        private static IEnumerable<Problem> GetPreconfiguredProblems()
        {
            return new List<Problem>
            {
                new Problem() { Title = "a", KnowledgeSpaceId = 1, X = 276.346221923828, Y = 110.263603210449 },
                new Problem() { Title = "b", KnowledgeSpaceId = 1, X = 277.467407226563, Y = 411.865142822266 },
                new Problem() { Title = "c", KnowledgeSpaceId = 1, X = 649.704223632813, Y = 190.989669799805 },
                new Problem() { Title = "d", KnowledgeSpaceId = 1, X = 905.336730957031, Y = 389.441223144531 },
                new Problem() { Title = "e", KnowledgeSpaceId = 1, X = 629.522705078125, Y = 577.802001953125 }
            };
        }

        private static IEnumerable<Edge> GetPreconfiguredEdges()
        {
            return new List<Edge>
            {
                new Edge() { ProblemSourceId = 2, ProblemTargetId = 3, KnowledgeSpaceId = 1 },
                new Edge() { ProblemSourceId = 1, ProblemTargetId = 3, KnowledgeSpaceId = 1 },
                new Edge() { ProblemSourceId = 1, ProblemTargetId = 5, KnowledgeSpaceId = 1 },
                new Edge() { ProblemSourceId = 5, ProblemTargetId = 4, KnowledgeSpaceId = 1 },
                new Edge() { ProblemSourceId = 3, ProblemTargetId = 4, KnowledgeSpaceId = 1 },
            };
        }

        private static IEnumerable<Test> GetPreconfiguredTests()
        {
            return new List<Test>
            {
                new Test() {Title = "Manifesto", Description = "Let the ruling classes tremble at a Communistic revolution. The proletarians have nothing to lose but their chains. They have a world to win.",
                            ProfessorId = 1,
                            KnowledgeSpaceId = 1,
                            Questions = new Collection<Question>(){
                                new Question()
                                {
                                    Text = "What is Manifesto?",
                                    IsMultipleChoice = false,
                                    ProblemId = 1,
                                    Answers = new Collection<Answer>()
                                    {
                                        new Answer(){ Text = "Communism is the doctrine of the conditions of the liberation of the proletariat.", Correct = true},
                                        new Answer(){ Text = "Communism is class in society which lives entirely from the sale of its labor and does not draw profit from any kind of capital.", Correct = false},
                                        new Answer(){ Text = "Communism is an economic and political system in which a country's trade and industry are controlled by private owners for profit, rather than by the state.", Correct = false},
                                        new Answer(){ Text = "Communism is a social system in which the nobility holds lands from the Crown in exchange for military service, and vassals are in turn tenants of the nobles, while the peasants are obliged to live on their lord's land and give him homage, labour, and a share of the produce, notionally in exchange for military protection.", Correct = false}
                                    }
                                },
                                new Question()
                                {
                                    Text = "What is Communism?",
                                    IsMultipleChoice = false,
                                    ProblemId = 2,
                                    Answers = new Collection<Answer>()
                                    {
                                        new Answer(){ Text = "Communism is the doctrine of the conditions of the liberation of the proletariat.", Correct = true},
                                        new Answer(){ Text = "Communism is class in society which lives entirely from the sale of its labor and does not draw profit from any kind of capital.", Correct = false},
                                        new Answer(){ Text = "Communism is an economic and political system in which a country's trade and industry are controlled by private owners for profit, rather than by the state.", Correct = false},
                                        new Answer(){ Text = "Communism is a social system in which the nobility holds lands from the Crown in exchange for military service, and vassals are in turn tenants of the nobles, while the peasants are obliged to live on their lord's land and give him homage, labour, and a share of the produce, notionally in exchange for military protection.", Correct = false}
                                    }
                                }
                            }
                },
                //new Test() {Title = "Spring boot vs .net core", Description = "With the thaw in the Cold War that erupted between Sun and Microsoft in 2000, and Microsoft’s embracing Java as a tool for building applications on top of Azure, attitudes in the .NET-leaning camp have changed somewhat significantly toward Java.",
                //            Professor = new Professor() {Username = "java", FirstName = "James", LastName = "Gosling", Password = "123456"  },
                //            KnowledgeSpaceId = 1,
                //            Questions = new Collection<Question>(){
                //                new Question()
                //                {
                //                    Text = "What is Java?",
                //                    IsMultipleChoice = false,
                //                    Answers = new Collection<Answer>()
                //                    {
                //                        new Answer(){ Text = "Java is a class-based, object-oriented programming language that is designed to have as few implementation dependencies as possible..", Correct = true},
                //                        new Answer(){ Text = "Java is is a general-purpose, procedural computer programming language supporting structured programming, lexical variable scope, and recursion, with a static type system.", Correct = false},
                //                        new Answer(){ Text = "Java is a general-purpose, multi-paradigm programming language encompassing static typing, strong typing, lexically scoped, imperative, declarative, functional, generic, object-oriented (class-based), and component-oriented programming disciplines.", Correct = false},
                //                        new Answer(){ Text = "Java is an island of Indonesia, bordered by the Indian Ocean on the south and the Java Sea on the north.", Correct = false}
                //                    }
                //                }
                //            }
                //},
                //new Test() {Title = "Angular vs react", Description = "Angular is an open-source front-end framework based on TypeScript, and it was rewritten from AngularJS, the JavaScript-based web framework.",
                //            Professor = new Professor() {Username = "javascript", FirstName = "Brendan", LastName = "Eich", Password = "123456"  },
                //            KnowledgeSpaceId = 1,
                //            Questions = new Collection<Question>(){
                //                new Question()
                //                {
                //                    Text = "What is TypeScript?",
                //                    IsMultipleChoice = false,
                //                    Answers = new Collection<Answer>()
                //                    {
                //                        new Answer(){ Text = "Typescript is a programming language that conforms to the ECMAScript specification.", Correct = false},
                //                        new Answer(){ Text = "Typescript is a command language written by Brian Fox for the GNU Project as a free software replacement for the Bourne shell.", Correct = false},
                //                        new Answer(){ Text = "TypeScript is a programming language developed and maintained by Microsoft.", Correct = true}
                //                    }
                //                }
                //            }
                //}
            };
        }
        public static void ClearDatabase(DataContext context)
        {
            context.Students.RemoveRange(context.Students);
            context.Professors.RemoveRange(context.Professors);
            context.Tests.RemoveRange(context.Tests);
            context.Questions.RemoveRange(context.Questions);
            context.Answers.RemoveRange(context.Answers);
            context.Enrolements.RemoveRange(context.Enrolements);
            context.EnrolementAnswers.RemoveRange(context.EnrolementAnswers);
            context.KnowledgeSpaces.RemoveRange(context.KnowledgeSpaces);
            context.Problems.RemoveRange(context.Problems);
            context.Edges.RemoveRange(context.Edges);
            context.PossibleStatesWithPossibilities.RemoveRange(context.PossibleStatesWithPossibilities);
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Students', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Professors', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Tests', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Questions', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Answers', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Enrolements', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('EnrolementAnswers', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('KnowledgeSpaces', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Problems', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('Edges', RESEED, 0)");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT('PossibleStatesWithPossibilities', RESEED, 0)");

        }
    }
}
