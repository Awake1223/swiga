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
    public class ReservationConfiguration : IEntityTypeConfiguration<ReservationEntity>
    {
        public void Configure(EntityTypeBuilder<ReservationEntity> builder)
        {
            builder.HasKey(r => r.Id);

            // Индексы для быстрого поиска
            builder.HasIndex(r => r.ClientId);
            builder.HasIndex(r => r.RentalPointId);
            builder.HasIndex(r => r.Status);
            builder.HasIndex(r => r.StartDate);
            builder.HasIndex(r => r.EndDate);
            builder.HasIndex(r => new { r.RentalPointId, r.Status });

            // Свойства
            builder.Property(r => r.StartDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(r => r.EndDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(r => r.Status)
                .IsRequired()
                .HasConversion<int>(); // Сохраняем Enum как int

            builder.Property(r => r.TotalPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(r => r.CreatedAt)
                .IsRequired()
                .HasColumnType("timestamp with time zone");

            builder.Property(r => r.ConfirmedAt)
                .HasColumnType("timestamp with time zone");

            builder.Property(r => r.CancelledAt)
                .HasColumnType("timestamp with time zone");

            builder.Property(r => r.CancellationReason)
                .HasMaxLength(500);

            // Внешние ключи и связи
            builder.HasOne(r => r.Client)
                .WithMany() // У клиента может быть много бронирований
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Restrict); // Запрещаем каскадное удаление

            builder.HasOne(r => r.RentalPoint)
                .WithMany(rp => rp.Reservations) // Добавьте это свойство в RentalPointEntity
                .HasForeignKey(r => r.RentalPointId)
                .OnDelete(DeleteBehavior.Restrict);

            // Связь с позициями
            builder.HasMany(r => r.Items)
                .WithOne(ri => ri.Reservation)
                .HasForeignKey(ri => ri.ReservationId)
                .OnDelete(DeleteBehavior.Cascade); // При удалении брони удаляем позиции
        }
    }
    }
