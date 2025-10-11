using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Infrastructure.Entity;

namespace Swiga.Infrastructure.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
    {
        public void Configure(EntityTypeBuilder<ClientEntity> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.DateOfBirth)
                .IsRequired(false);

            builder.Property(c => c.PassportData)
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(c => c.DriverLicense)
                .HasMaxLength(50)
                .IsRequired(false);

            // Связь с User
            builder.HasOne(c => c.User)
                .WithOne()
                .HasForeignKey<ClientEntity>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
