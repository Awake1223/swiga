using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Domain.Models;
using Swiga.Infrastructure.Entity;


namespace Swiga.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(u => u.CreatedAt)
                .IsRequired();

            builder.Property(u => u.Role)
                .IsRequired()
                .HasConversion<string>(); 

            // Индексы
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasIndex(u => u.PhoneNumber).IsUnique();

        }
    }

}

