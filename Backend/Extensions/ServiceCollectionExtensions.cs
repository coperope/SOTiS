using Backend.CQRS.Processors;
using Backend.Data.Repositories;
using Backend.Data.Repositories.Interfaces;
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


            return services;
        }

        public static IServiceCollection UseAutoMapper(this IServiceCollection services)
        {
            // Automapper dependency injection configuration

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
