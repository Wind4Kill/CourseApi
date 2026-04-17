using System;
using CourseApiDomain;
using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApi;

public static class ProgramExtensions
{


      public static async Task MigratePendingMigrations(this WebApplication app)
      {
            await using (var scope = app.Services.CreateAsyncScope())
            {
                  var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
                  if(context.Database.GetPendingMigrations().Any())
                  {
                        await context.Database.MigrateAsync();
                  }

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
                                   Author =  new Author() { Name = "Andrew Troelsen" } ,
                                   Categories = new List<Category>() { new Category { Name = "C#" } },
                                   Reviews = new List<Review>() { new Review { ReviewText = "Great course!", ReviewRating = 10.0 } }
                             }
                       );

                        await context.SaveChangesAsync();
                  }
            }
      }
}
