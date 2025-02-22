﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReservationManager.DomainModel.Operation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Configurations
{
    internal class ClosingCalendarConfiguration : IEntityTypeConfiguration<ClosingCalendar>
    {
        public void Configure(EntityTypeBuilder<ClosingCalendar> builder)
        {
            builder.ToTable("ClosingCalendar");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Day)
                   .IsRequired();
            
            builder.HasOne(x => x.Resource)
                   .WithMany()
                   .IsRequired()
                   .HasForeignKey(x => x.ResourceId)
                   .OnDelete(DeleteBehavior.NoAction);
            
            builder.Property(x => x.Description)
                   .HasMaxLength(500);

            builder.HasQueryFilter(x => !x.IsDeleted.HasValue);

        }
    }
}
