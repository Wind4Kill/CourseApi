
using CourseApi.Application.Authentication;
using CourseApi.Application.Authors.Services;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseApi.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IAuthorService, AuthorService>();
            services.Configure<JwtTokenSettings>(configuration.GetSection("JwtSettings"));

            return services;
        }
    }
}