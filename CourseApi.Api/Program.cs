using CourseApi.Enpoints;
using System.Diagnostics;
using CourseApi;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using CourseApi.Api;
using FluentValidation;
using CourseApi.Application;
using CourseApi.Api.HelpClasses;
using CourseApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.ConfigureHttpJsonOptions(options =>
{
      options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

if (builder.Environment.IsDevelopment())
{
      builder.Services.AddDistributedMemoryCache();
}

if (builder.Environment.IsProduction())
{
      builder.Services.AddStackExchangeRedisCache(options =>
      {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "CourseApi_cache";
      });

      builder.Services.AddStackExchangeRedisOutputCache(options =>
      {
            options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
            options.InstanceName = "CourseApi_cache";
      });
}

builder.Services.AddOutputCache();
builder.Services.AddProblemDetails();
builder.Services.AddApplication();
builder.Services.AddData(builder.Configuration.GetConnectionString("PostgreConnection")!);

//remove IsProduction in production
if (builder.Environment.IsDevelopment() || builder.Environment.IsProduction())
{
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
      builder.Services.AddHealthChecks();
}


var app = builder.Build();

app.UseStatusCodePages();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler();
      await app.MigratePendingMigrations();
}

//remove IsProduction in production
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
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
