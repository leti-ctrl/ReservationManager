using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Configurations
{
    internal class BuildingTimetableConfiguration : IEntityTypeConfiguration<BuildingTimetable>
    {
        public void Configure(EntityTypeBuilder<BuildingTimetable> builder)
        {
            builder.ToTable("BuildingTimetable");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Type)
                .WithMany()
                .IsRequired()
                .HasForeignKey(x => x.TypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(x => x.StartDate);
            
            builder.Property(x => x.EndDate);
            
            builder.Property(x => x.StartTime);
            
            builder.Property(x => x.EndTime);
            
            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);

            builder.Navigation(e => e.Type).AutoInclude();
        }
    }
}
