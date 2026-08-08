using CourseApi.Enpoints;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CourseApi;
using System.Text.Json.Serialization;
using CourseApiDomain;
using Microsoft.EntityFrameworkCore;
using CourseApi.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.ConfigureHttpJsonOptions(options =>
{
      options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.AddMemoryCache();
if (builder.Environment.IsProduction())
{
      builder.Services.AddStackExchangeRedisOutputCache(options =>
      {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "CourseApi_cache";
      });
}

builder.Services.AddOutputCache();
builder.Services.AddProblemDetails();
builder.Services.AddServices();

//remove IsProduction in production
if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
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

var app = builder.Build();
app.UseStatusCodePages();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler();
      await app.MigratePendingMigrations();
}

//remove IsProduction in production
if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
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
