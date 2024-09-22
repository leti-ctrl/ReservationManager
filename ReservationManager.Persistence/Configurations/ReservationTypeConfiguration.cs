using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Persistence.Configurations
{
    public class ReservationTypeConfiguration : IEntityTypeConfiguration<ReservationType>
    {
        public void Configure(EntityTypeBuilder<ReservationType> builder)
        {
            builder.ToTable("ReservationType");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(20);

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);
        }
    }
}
