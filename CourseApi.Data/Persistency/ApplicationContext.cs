using System.Reflection;
using CourseApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseApi.Data.Persistency.Repositories;

public class ApplicationContext : IdentityDbContext<User>
{
      public DbSet<Course> Courses { get; set; }
      public DbSet<Author> Authors { get; set; }
      public DbSet<Category> Categories { get; set; }
      public DbSet<RefreshToken> RefreshTokens { get; set; }

      public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }


      protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
      {
            configurationBuilder.Properties<string>().HaveMaxLength(100);
      }

      protected override void OnModelCreating(ModelBuilder modelBuilder)
      {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
      }
      public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
      {
            var courses = ChangeTracker.Entries<Course>().
            Where(c => c.State == EntityState.Modified || c.State == EntityState.Added).ToList();

            foreach (var course in courses)
            {
                  course.Entity.Version = Guid.NewGuid();
            }

           return await base.SaveChangesAsync(cancellationToken);
      }

}