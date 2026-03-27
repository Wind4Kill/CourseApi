using CourseApi.Enpoints;
using CourseApiDomain;
using CourseApiServices;
using CourseApiServices.Interfaces;
using CourseApiServices.Interfaces.Repositories;
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
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

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
      await using (var scope = app.Services.CreateAsyncScope())
      {
            ApplicationContext _context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            
            if(_context.Database.GetPendingMigrations().Any())
            {
                  await _context.Database.MigrateAsync();
            }
      }
}

app.AddCourseEndpoints();
app.AddAuthorEndpoins();

app.Run();
