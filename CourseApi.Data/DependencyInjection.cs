using System.Diagnostics;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Data.Persistency.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CourseApi.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseNpgsql(connectionString, npsqlOptions => npsqlOptions.EnableRetryOnFailure().SetPostgresVersion(18, 6))
                    .LogTo((message) => Debug.WriteLine(message), LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            });
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            
            return services;
        }
    }
}