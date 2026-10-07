using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CourseApi.Data.Persistency.Configurations
{
    public class RefreshTokenConfig : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasOne(rt => rt.User).WithMany(u=>u.RefreshTokens).HasForeignKey(rt => rt.UserId);
            builder.Property(rt => rt.Token).HasMaxLength(200);
        }
    }
}