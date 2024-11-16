using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Persistence.Configurations
{
    public class UserTypeConfigration : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserType");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);
            
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);
        }
    }
}
