using System.Buffers;
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
using CourseApi.Api.Endpoints;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
builder.Services.ConfigureHttpJsonOptions(options =>
{
      options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddAuthentication(options =>
{
      options.DefaultAuthenticateScheme = BearerTokenDefaults.AuthenticationScheme;
      options.DefaultChallengeScheme = BearerTokenDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
      options.TokenValidationParameters = new()
      {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecurityKey"]!)),
            ClockSkew = TimeSpan.Zero
      };
});

builder.Services.AddAuthorization(options =>
{
      options.AddPolicy("IsAdmin", policy => policy.RequireClaim("UserName", builder.Configuration["AdminCredentials:UserName"]!).RequireClaim("Role", "Admin"));
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
builder.Services.AddApplication(builder.Configuration);
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
await app.AddAdmin();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler();
      await app.MigratePendingMigrations();
}
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

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
app.AddUserEndpoints();
app.UseOutputCache();


app.Run();
