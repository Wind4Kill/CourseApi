using System;
using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApiDomain;

public class CourseTypeConfiguration : IEntityTypeConfiguration<Course>
{
      public void Configure(EntityTypeBuilder<Course> builder)
      {
            builder.HasIndex(p => p.CourseName).IsUnique();
            builder.OwnsOne(c => c.CourseDetails);
            builder.Navigation(c => c.CourseDetails).IsRequired();
            builder.HasQueryFilter(c => !c.IsDeleted);

            builder.Ignore(c => c.AverageRating);
      }
}
