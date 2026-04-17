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
using CourseApi;

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
      await app.MigratePendingMigrations();
}

app.UseStatusCodePages();

if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
      app.UseSwaggerUI();
      app.MapHealthChecks("/health");
      await app.SeedData();

}

app.AddCourseEndpoints();
app.AddAuthorEndpoints();

app.Run();
