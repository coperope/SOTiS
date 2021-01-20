using AutoMapper;
using Backend.CQRS.Processors;
using Backend.Data.Repositories;
using Backend.Data.Repositories.Interfaces;
using Backend.Utils.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection UseRepositories(this IServiceCollection services)
        {
            // Repositories dependency injection definitions

            services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
            services.AddScoped(typeof(ITestRepository), typeof(TestRepository));
            services.AddScoped(typeof(IEnrolementRepository), typeof(EnrolementRepository));
            services.AddScoped(typeof(IEnrolementAnswerRepository), typeof(EnrolementAnswerRepository));
            services.AddScoped(typeof(IQuestionRepository), typeof(QuestionRepository));
            services.AddScoped(typeof(IAnswerRepository), typeof(AnswerRepository));
            services.AddScoped(typeof(IKnowledgeSpaceRepository), typeof(KnowledgeSpaceRepository));
            services.AddScoped(typeof(IPossibleStatesWithPossibilitiesRepository), typeof(PossibleStatesWithPossibilitiesRepository));
            return services;
        }

        public static IServiceCollection UseAutoMapper(this IServiceCollection services)
        {
            // Automapper dependency injection configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new Mapping());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }

        public static IServiceCollection UseOtherDI(this IServiceCollection services)
        {
            services.AddTransient<ICommandProcessor, CommandProcessor>();
            services.AddTransient<IQueryProcessor, QueryProcessor>();


            return services;
        }
    }
}
