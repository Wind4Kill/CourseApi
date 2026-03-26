using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApiDomain;

public class ApplicationContext : DbContext
{
      public DbSet<Course> Courses { get; set; }
      public DbSet<Author> Authors { get; set; }
      public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            modelBuilder.Entity<Course>().OwnsOne(c => c.CourseDetails);
            modelBuilder.Entity<Course>().Navigation(c => c.CourseDetails).IsRequired();
      }

}