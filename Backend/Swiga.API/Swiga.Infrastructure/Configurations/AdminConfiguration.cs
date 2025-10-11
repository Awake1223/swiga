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
    public class AdminConfiguration : IEntityTypeConfiguration<AdminEntity>
    {
        public void Configure(EntityTypeBuilder<AdminEntity> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.RentalPointId)
                .IsRequired();

            // Связь с User
            builder.HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<AdminEntity>(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь с RentalPoint
            builder.HasOne(a => a.RentalPoint)
                .WithMany()
                .HasForeignKey(a => a.RentalPointId);
        }
    }
}
