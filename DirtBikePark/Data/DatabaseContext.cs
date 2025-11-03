using DirtBikePark.Models;
using Microsoft.EntityFrameworkCore;

namespace DirtBikePark.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Park> Parks { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DatbaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    }
}
