using Microsoft.EntityFrameworkCore;

using RailcarTrips.Infrastructure.Entities;

namespace RailcarTrips.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<TripEntity> Trips => Set<TripEntity>();
        public DbSet<EquipmentEventEntity> EquipmentEvents => Set<EquipmentEventEntity>();
        public DbSet<CityEntity> Cities => Set<CityEntity>();

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TripEntity>()
                .HasMany(t => t.Events)
                .WithOne(e => e.Trip)
                .HasForeignKey(e => e.TripId);
        }
    }
}
