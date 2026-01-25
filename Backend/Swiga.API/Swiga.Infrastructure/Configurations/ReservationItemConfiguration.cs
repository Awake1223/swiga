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
    public class ReservationItemConfiguration : IEntityTypeConfiguration<ReservationItemEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationItemEntity> builder)
        {
            builder.HasKey(ri => ri.Id);

            // Индексы
            builder.HasIndex(ri => ri.ReservationId);
            builder.HasIndex(ri => ri.InventoryId);
            builder.HasIndex(ri => new { ri.ReservationId, ri.InventoryId });

            // Свойства
            builder.Property(ri => ri.Size)
                .IsRequired();

            builder.Property(ri => ri.Gender)
                .IsRequired()
                .HasConversion<int>(); // Enum как int

            builder.Property(ri => ri.Quantity)
                .IsRequired();

            builder.Property(ri => ri.PricePerHour)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(ri => ri.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            // Внешние ключи
            builder.HasOne(ri => ri.Reservation)
                .WithMany(r => r.Items)
                .HasForeignKey(ri => ri.ReservationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ri => ri.Inventory)
                .WithMany() // У инвентаря может быть много бронирований
                .HasForeignKey(ri => ri.InventoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
