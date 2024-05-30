using Microsoft.EntityFrameworkCore;
using TravelSystem.Models;

namespace TravelSystem
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Bus> Buss { get; set; }
        public DbSet<Clint> Clints { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public DbSet<TripKind> TripKinds { get; set; }
    }
}
