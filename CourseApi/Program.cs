using CourseApi.Enpoints;
using CourseApiDomain;
using CourseApiServices;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using CourseApiDomain.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails(options =>
{
      options.Map<InvalidOperationException>(ex => new ProblemDetails
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
}

string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<ApplicationContext>(options =>
{
      options.UseSqlite(connection);
});

builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
      app.UseSwaggerUI();
}

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
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

app.AddCourseEndpoints();
app.AddAuthorEndpoints();

app.Run();
