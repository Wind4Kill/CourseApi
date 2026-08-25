using System;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApi.Data.Persistency.Configurations;

public class AuthorTypeConfiguration : IEntityTypeConfiguration<Author>
{
      public void Configure(EntityTypeBuilder<Author> builder)
      {
            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasQueryFilter(a => !a.IsDeleted);
      }
}

