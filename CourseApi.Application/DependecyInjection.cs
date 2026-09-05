
using CourseApi.Application.Authors.Services;
using CourseApi.Application.Interfaces.Services;
using CourseApi.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CourseApi.Application
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICourseService, CourseService>();
            services.AddScoped<IAuthorService, AuthorService>();

            return services;
        }
    }
}