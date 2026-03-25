using CourseApi.Enpoints;
using CourseApiDomain;
using CourseApiServices;
using CourseApiServices.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();

if (builder.Environment.IsDevelopment())
{
      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();
}

string connection = builder.Configuration.GetConnectionString("Database Connection")!;
builder.Services.AddDbContext<ApplicationContext>(options =>
{
      options.UseSqlite(connection);
});

builder.Services.AddTransient<ICourseService, CourseService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
      app.UseSwagger();
      app.UseSwaggerUI();
}

app.UseStatusCodePages();

if (app.Environment.IsProduction())
{
      app.UseExceptionHandler();
}

app.AddCourseEndpoints();
app.AddAuthorEndpoins();

app.Run();
