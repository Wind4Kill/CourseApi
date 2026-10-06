using System.Diagnostics;
using CourseApi.Application.Interfaces.Repositories;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Data.Caching;
using CourseApi.Data.Persistency;
using CourseApi.Data.Persistency.Repositories;
using CourseApi.Domain.Entities;
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

            services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<ApplicationContext>();
            
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped(typeof(ICacheService<>), typeof(CacheService<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            return services;
        }
    }
}