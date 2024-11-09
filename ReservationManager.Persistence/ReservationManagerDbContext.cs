using Microsoft.EntityFrameworkCore;
using ReservationManager.Persistence.Configurations;

namespace ReservationManager.Persistence
{
    public class ReservationManagerDbContext : DbContext
    {
        public ReservationManagerDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ReservationConfiguration());
            modelBuilder.ApplyConfiguration(new ReservationTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceConfiguration());
            modelBuilder.ApplyConfiguration(new ResourceTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserTypeConfigration());
            modelBuilder.ApplyConfiguration(new TimetableTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BuildingTimetableConfiguration());
        }

    }
}
