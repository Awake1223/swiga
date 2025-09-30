

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Domain.Models;


namespace Swiga.Infrastructure.Configurations
{
    public class RentalPointConfiguration : IEntityTypeConfiguration<RentalPointModel>
    {
        public void Configure (EntityTypeBuilder<RentalPointModel> builder)
        {
            builder.HasKey(r => r.Id);
        }
    }
}
