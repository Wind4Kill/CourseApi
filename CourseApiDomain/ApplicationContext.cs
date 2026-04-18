using System.Reflection;
using CourseApiDomain.Entities;
using CourseApiDomain.Views;
using Microsoft.EntityFrameworkCore;

namespace CourseApiDomain;

public class ApplicationContext : DbContext
{
      public DbSet<Course> Courses { get; set; }
      public DbSet<Author> Authors { get; set; }

      public DbSet<Category> Categories { get; set; }

      public DbSet<CourseRating> Ratings { get; set; }
      public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


      protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
      {
            configurationBuilder.Properties<string>().HaveMaxLength(100);
      }


      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.HasDbFunction(() => CourseFunctions.GetCourseRating(default(int))).
            HasName("get_course_rating").HasSchema("public");

            modelBuilder.Entity<CourseRating>().HasNoKey().ToSqlQuery("""
              SELECT 
                c."CourseId",
                get_course_rating(c."CourseId") AS "AvgRating"
            FROM "Courses" c
            """);
      }

}