using Microsoft.EntityFrameworkCore;
using BookingAPI.Data;
using BookingAPI.Models;
using BookingAPI.DTOs;

namespace BookingAPI.Services;

public class BookingService(AppDbContext db, ConflictDetectionService conflicts, NotificationService notifs)
{
    public async Task<Booking> Create(CreateBookingRequest req)
    {
        if (await conflicts.HasConflict(req.RoomId, req.StartTime, req.EndTime))
            throw new InvalidOperationException("Time slot conflict detected");

        var b = new Booking
        {
            RoomId = req.RoomId,
            Title = req.Title,
            BookedBy = req.BookedBy,
            StartTime = req.StartTime,
            EndTime = req.EndTime,
            Status = BookingStatus.Confirmed
        };
        db.Bookings.Add(b);
        await db.SaveChangesAsync();
        await notifs.Create(b.Id, $"Booking '{b.Title}' confirmed for {b.StartTime:g}-{b.EndTime:t}");
        return b;
    }

    public async Task<Booking?> Cancel(int id)
    {
        var b = await db.Bookings.FindAsync(id);
        if (b is null) return null;
        b.Status = BookingStatus.Cancelled;
        await db.SaveChangesAsync();
        await notifs.Create(b.Id, $"Booking '{b.Title}' has been cancelled");
        return b;
    }

    public async Task<List<Booking>> GetAll() =>
        await db.Bookings.Include(b => b.Room).OrderBy(b => b.StartTime).ToListAsync();

    public async Task<Booking?> GetById(int id) =>
        await db.Bookings.Include(b => b.Room).FirstOrDefaultAsync(b => b.Id == id);

    public async Task<List<Booking>> GetByRoom(int roomId) =>
        await db.Bookings.Include(b => b.Room)
            .Where(b => b.RoomId == roomId && b.Status != BookingStatus.Cancelled)
            .OrderBy(b => b.StartTime).ToListAsync();
}
