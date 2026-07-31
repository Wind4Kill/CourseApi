using CourseApi.Enpoints;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CourseApi;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using CourseApiDomain;
using Microsoft.EntityFrameworkCore;
using CourseApiServices.Interfaces.HelpClasses;
using CourseApiServices.HelpClasses.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
      options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddMemoryCache();
builder.Services.AddProblemDetails();
builder.Services.AddServices();

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

var app = builder.Build();
app.UseStatusCodePages();

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
