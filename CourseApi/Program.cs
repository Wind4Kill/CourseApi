using CourseApi.Enpoints;
using CourseApiDomain;
using CourseApiServices;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using CourseApiDomain.Entities;
using System.Reflection;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.Interfaces.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails(options =>
{
      options.Map<EntityNotFoundException>(ex => new ProblemDetails
      {
            Detail = ex.Message,
            Status = StatusCodes.Status404NotFound,
            Title = "Entity wasn't found"
      });
});


if (builder.Environment.IsDevelopment())
{
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddHealthChecks();
}

string connection = builder.Configuration.GetConnectionString("PostgreConnection")!;
builder.Services.AddDbContext<ApplicationContext>(options =>
{
      options.UseNpgsql(connection, options =>
      options.EnableRetryOnFailure());

      if (!builder.Environment.IsProduction())
      {
            options.LogTo((message) => Debug.WriteLine(message), LogLevel.Information)
            .EnableSensitiveDataLogging().
            EnableDetailedErrors();
      }


});

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler();
      await using (var scope = app.Services.CreateAsyncScope())
      {
            ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                  await context.Database.MigrateAsync();
            }
      }
}

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
      app.UseSwaggerUI();
      app.MapHealthChecks("/health");

      await using (var scope = app.Services.CreateAsyncScope())
      {
            ApplicationContext context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            if (!context.Courses.Any())
            {
                  context.Courses.AddRange(
                        new Course()
                        {
                              CourseName = "C#",
                              CourseDetails = new CourseDetails
                              {
                                    CourseDescription = "Advanced C#",
                                    CoursePrice = 1000
                              },
                              Authors = new List<Author>() { new Author() { Name = "Andrew Troelsen" } },
                              Categories = new List<Category>() { new Category { Name = "C#" } },
                              Reviews = new List<Review>() { new Review { ReviewText = "Great course!", ReviewRating = 10.0 } }
                        }
                  );

                  await context.SaveChangesAsync();
            }
      }
}

app.AddCourseEndpoints();
app.AddAuthorEndpoints();

app.Run();
