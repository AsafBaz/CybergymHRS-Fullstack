using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using CybergymHRS.Core.Models;
using CybergymHRS.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CybergymHRS.Infrastructure.Data
{
    public class CybergymHrsContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public CybergymHrsContext(DbContextOptions<CybergymHrsContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Room>()
                .HasIndex(r => r.RoomNumber)
                .IsUnique();
            modelBuilder.Entity<Reservation>()
                .HasCheckConstraint("CHK_Date_Valid", "[CheckOutDate] > [CheckInDate]");
        }
    }
}
