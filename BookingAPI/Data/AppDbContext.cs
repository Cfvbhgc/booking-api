using Microsoft.EntityFrameworkCore;
using BookingAPI.Models;

namespace BookingAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<TimeSlot> TimeSlots => Set<TimeSlot>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Booking>()
            .HasIndex(b => new { b.RoomId, b.StartTime, b.EndTime })
            .IsUnique()
            .HasFilter("\"Status\" != 2");

        mb.Entity<Booking>()
            .Property(b => b.Status)
            .HasConversion<int>();

        mb.Entity<Room>().HasData(
            new Room { Id = 1, Name = "Main Hall", Capacity = 100, Location = "Building A, Floor 1", IsActive = true },
            new Room { Id = 2, Name = "Meeting Room 1", Capacity = 10, Location = "Building A, Floor 2", IsActive = true },
            new Room { Id = 3, Name = "Conference Room", Capacity = 30, Location = "Building B, Floor 1", IsActive = true }
        );

        mb.Entity<TimeSlot>().HasData(
            new TimeSlot { Id = 1, RoomId = 1, DayOfWeek = 1, StartHour = 8, EndHour = 20 },
            new TimeSlot { Id = 2, RoomId = 2, DayOfWeek = 1, StartHour = 9, EndHour = 18 },
            new TimeSlot { Id = 3, RoomId = 3, DayOfWeek = 1, StartHour = 8, EndHour = 22 }
        );
    }
}
