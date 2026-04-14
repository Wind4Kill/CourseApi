using System;
using CourseApiDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApiDomain;

public class AuthorTypeConfiguration : IEntityTypeConfiguration<Author>
{
      public void Configure(EntityTypeBuilder<Author> builder)
      {
            builder.HasQueryFilter(a => a.IsDeleted != true);
      }
}

