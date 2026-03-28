using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseApiDomain;

public class ApplicationContext : DbContext
{
      public DbSet<Course> Courses { get; set; }
      public DbSet<Author> Authors { get; set; }
      public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }

      public double? GetCourseRating(int courseId)
      {
            return null;
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            modelBuilder.ApplyConfiguration(new CourseTypeConfiguration());

            modelBuilder.Entity<Course>()
            .HasData(
                  new
                  {

                        CourseId = 5,
                        CourseName = "AspNet Core",
                        
                        IsDeleted = false

                  });

            modelBuilder.Entity<Author>().HasData(
                  new
                  {
                        AuthorId = 4,
                        Name = "John Doe",
                        CourseId = 5
                  });

            modelBuilder.Entity<Category>().HasData(new
            {
                  CategoryId = 5,
                  Name = "Backend",
                  CourseId = 5
            });

            modelBuilder.Entity<Course>().OwnsOne(c => c.CourseDetails).HasData(new
            {
                  CourseId = 5,
                  CourseDescription = "AspNetCore essentials",
                  CoursePrice = 1000.0m
            });

            modelBuilder.Entity<Review>().HasData(new
            {
                  ReviewId = 1,
                  ReviewText = "Great course!",
                  ReviewRating = 10.0,
                  CourseId=5
            });

            modelBuilder.HasDbFunction(() => GetCourseRating(default(int)));

      }

}