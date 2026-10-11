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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration, builder.Environment);

builder.Host.UseSerilog((context, configuration) =>
{
      configuration.ReadFrom.Configuration(context.Configuration);
});

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddData(builder.Configuration.GetConnectionString("PostgreConnection")!);


var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseStatusCodePages();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler();
      var forwardedHeadersOptions = new ForwardedHeadersOptions()
      {
            ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor |
              Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto

      };
      forwardedHeadersOptions.KnownIPNetworks.Clear();
      forwardedHeadersOptions.KnownIPNetworks.Clear();

      app.UseForwardedHeaders(forwardedHeadersOptions);
      await app.MigratePendingMigrations();
      // app.UseHsts();
}

// app.UseHttpsRedirection();
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
      await app.AddAdmin();
}

app.AddCourseEndpoints();
app.AddAuthorEndpoints();
app.AddUserEndpoints();
app.UseOutputCache();


app.Run();
