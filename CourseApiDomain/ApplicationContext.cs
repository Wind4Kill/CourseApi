using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApiDomain;

public class ApplicationContext : DbContext
{
      public DbSet<Course> Courses { get; set; }
      public DbSet<Author> Authors { get; set; }
      public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

      public static double? GetCourseRating(int courseId)
      {
            throw new NotImplementedException();
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            modelBuilder.ApplyConfiguration(new CourseTypeConfiguration());

            modelBuilder.HasDbFunction(() => GetCourseRating(default(int))).
            HasName("get_course_rating").HasSchema("public");
      }

}