using Microsoft.EntityFrameworkCore;

namespace Parking.Api.Models
{
    public class ParkingContext : DbContext
    {
        public ParkingContext(DbContextOptions options)
            : base(options)
        { }

        public DbSet<Parking> Parking { get; set; }
        public DbSet<ParkingPlace> ParkingPlaces { get; set; }
        public DbSet<Command> CommandStore { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Parking>()
                .HasKey(p => p.Name);
            modelBuilder.Entity<Parking>()
                .HasMany(p => p.Places)
                .WithOne(p => p.Parking)
                .HasForeignKey(p => p.ParkingName)
                .IsRequired();

            modelBuilder.Entity<ParkingPlace>()
                .HasKey(p => new { p.ParkingName, p.Number });

            modelBuilder.Entity<Command>()
                .HasKey(c => c.Id);
        }
    }
}
