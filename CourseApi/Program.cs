using CourseApi.Enpoints;
using CourseApiDomain;
using CourseApiServices;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using CourseApiDomain.Entities;
using System.Reflection;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Diagnostics;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.Interfaces.Services;
using CourseApi;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using CourseApiServices.HelpClasses.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
      options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

if (builder.Environment.IsProduction())
{
      builder.Services.AddStackExchangeRedisOutputCache(options =>
      {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnectionString");
            options.InstanceName = "CourseApi_cache";
      });
}

builder.Services.AddOutputCache();
builder.Services.AddProblemDetails();

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

      if (!builder.Environment.IsDevelopment())
      {
            options.LogTo((message) => Debug.WriteLine(message), LogLevel.Information)
            .EnableSensitiveDataLogging().
            EnableDetailedErrors();
      }


});

builder.Services.AddServices();

var app = builder.Build();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler(app => app.Run(async context =>
      {
            var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            var (status, title) = exception switch
            {
                  EntityNotFoundException => (404, "Requested entity wasn't found"),
                  EntityAlreadyExistsExceptions => (409, "Requested entity wasn't found"),
                  _ => (500, "Internal Server Error")
            };

            context.Response.StatusCode = status;

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                  Title = title,
                  Status = status,
                  Detail = exception?.Message
            });

      }));
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
app.UseOutputCache();


app.Run();
