using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApi.Data.Persistency.Configurations
{
      public class ReviewConfig : IEntityTypeConfiguration<Review>
      {
            public void Configure(EntityTypeBuilder<Review> builder)
            {
            builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
            }
      }
}