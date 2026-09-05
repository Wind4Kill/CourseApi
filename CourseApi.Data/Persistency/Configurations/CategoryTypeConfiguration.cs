using System;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApi.Data.Persistency.Configurations;

public class CategoryTypeConfiguration : IEntityTypeConfiguration<Category>
{
      public void Configure(EntityTypeBuilder<Category> builder)
      {
            builder.HasIndex(c => c.Name).IsUnique();
      }
}
