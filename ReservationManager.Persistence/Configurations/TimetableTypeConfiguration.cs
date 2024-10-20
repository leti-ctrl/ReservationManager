using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Configurations
{
    public class TimetableTypeConfiguration : IEntityTypeConfiguration<TimetableType>
    {
        public void Configure(EntityTypeBuilder<TimetableType> builder)
        {
            builder.ToTable("TimetableType");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(20);

        }
    }
}
