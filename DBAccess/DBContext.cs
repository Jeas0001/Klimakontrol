using Microsoft.EntityFrameworkCore;
using Models;

namespace DBAccess
{
    public class DBContext : DbContext
    {
        public DbSet<Firm> Firms { get; set; }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=Minisvendeprøve;Trusted_Connection=Yes;TrustServerCertificate=True");
        }
    }
}
