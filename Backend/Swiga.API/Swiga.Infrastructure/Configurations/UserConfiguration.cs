using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Swiga.Domain.Models;


namespace Swiga.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserModel> 
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Email).IsRequired();
            builder.Property(u => u.Password).IsRequired();
            builder.Property(u => u.PhoneNumber).IsRequired();
            builder.Property(u => u.CreatedAt).IsRequired();
            builder.Property(u => u.Role).IsRequired();
       
            builder.HasDiscriminator<Role>(nameof(UserModel.Role))
                .HasValue<AdminModel>(Role.Admin)
                .HasValue<ClientModel>(Role.Client);

            builder.Property<string>("FullName")
                .IsRequired(false);

            builder.Property<Guid>("RentalPointId")
                .IsRequired(false);  // NOT NULL только для админов

            // Дополнительные свойства для ClientModel
            builder.Property<string>("FirstName")
                 .IsRequired(false); // NOT NULL только для клиентов

            builder.Property<string>("LastName")
                .IsRequired(false); // NOT NULL только для клиентов

            builder.Property<DateOnly?>("DateOfBirth")
                .IsRequired(false);

            builder.Property<string>("PassportData")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property<string>("DriverLicense")
                .HasMaxLength(50)
                .IsRequired(false);
        }

        
    }
}
