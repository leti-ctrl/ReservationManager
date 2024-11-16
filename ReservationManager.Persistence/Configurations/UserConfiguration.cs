using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Surname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasMany(x => x.Roles)
                .WithMany()
                .UsingEntity<RoleUser>(
                    x => x.HasOne<Role>().WithMany().OnDelete(DeleteBehavior.NoAction),
                    x => x.HasOne<User>().WithMany().OnDelete(DeleteBehavior.NoAction)
                );

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);
        }
    }
}
