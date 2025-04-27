using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Operation;
using System.Reflection.Emit;

namespace ReservationManager.Persistence.Configurations
{
    public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.ToTable("Reservations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(x => x.Description)
                .HasMaxLength(250);

            builder.Property(x => x.Day)
                .IsRequired();

            builder.Property(x => x.Start)
                .IsRequired();

            builder.Property(x => x.End)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Resource)
                .WithMany(r => r.Reservations)
                .IsRequired()
                .HasForeignKey(x => x.ResourceId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Type)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);

            builder.Navigation(e => e.Type).AutoInclude();
            builder.Navigation(e => e.User).AutoInclude();
            builder.Navigation(e => e.Resource).AutoInclude();
        }
    }
}
