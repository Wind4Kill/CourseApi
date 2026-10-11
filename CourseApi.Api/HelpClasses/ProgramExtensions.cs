using System.Security.Claims;
using CourseApi.Data.Persistency.Repositories;
using CourseApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseApi;

public static class ProgramExtensions
{

      public static async Task MigratePendingMigrations(this WebApplication app)
      {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                  var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                  var strategy = context.Database.CreateExecutionStrategy();

                  await strategy.ExecuteAsync(async () =>
                  {
                        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                        if (pendingMigrations.Any())
                        {
                              await context.Database.MigrateAsync();
                        }
                  });
            }
      }
      public static async Task SeedData(this WebApplication app)
      {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                  var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

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
                                   Author = new Author() { Name = "Andrew Troelsen" },
                                   Categories = new List<Category>() { new Category { Name = "C#" } }
                             }
                       );

                        await context.SaveChangesAsync();
                  }
            }
      }

      public static async Task AddAdmin(this WebApplication app)
      {
            await using var scope = app.Services.CreateAsyncScope();

            UserManager<User> userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            DbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var strategy = dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                  try

                  {
                        var transaction = await dbContext.Database.BeginTransactionAsync();
                        User admin = new(app.Configuration["AdminCredentials:UserName"]!)
                        {
                              Email = app.Configuration["AdminCredentials:Email"],
                        };

                        await userManager.CreateAsync(admin, app.Configuration["AdminCredentials:Password"]!);

                        Claim role = new Claim("Role", "Admin");

                        await userManager.AddClaimAsync(admin, role);

                        await transaction.CommitAsync();
                  }
                  catch (Exception)
                  {
                        throw;
                  }

            });

      }
}
