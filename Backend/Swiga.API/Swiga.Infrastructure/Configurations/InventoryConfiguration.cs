

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Domain.Models;


namespace Swiga.Infrastructure.Configurations
{
    public class InventoryConfiguration : IEntityTypeConfiguration<InventoryModel>
    {
        public void Configure(EntityTypeBuilder<InventoryModel> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name).IsRequired();
            builder.Property(i => i.Size).IsRequired();
            builder.Property(i => i.PricePerHour).IsRequired();
            builder.Property(i => i.Amount).IsRequired();
            builder.Property(i => i.Gender)
                .IsRequired()
                .HasConversion<string>();


        }
    }
}
