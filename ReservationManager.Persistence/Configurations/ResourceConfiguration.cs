using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Persistence.Configurations
{
    public class ResourceConfiguration : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {
            builder.ToTable("Resource");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.HasOne(x => x.Type)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);

            builder.Navigation(c => c.Type).AutoInclude();
        }
    }
}
