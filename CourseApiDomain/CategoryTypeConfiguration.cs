using System;
using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApiDomain;

public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
{
      public void Configure(EntityTypeBuilder<Category> builder)
      {
            builder.HasIndex(c => c.Name).IsUnique();
      }
}
