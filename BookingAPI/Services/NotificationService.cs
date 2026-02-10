using Microsoft.EntityFrameworkCore;
using BookingAPI.Data;
using BookingAPI.Models;

namespace BookingAPI.Services;

public class NotificationService(AppDbContext db)
{
    public async Task Create(int bookingId, string msg)
    {
        db.Notifications.Add(new Notification { BookingId = bookingId, Message = msg });
        await db.SaveChangesAsync();
    }

    public async Task<List<Notification>> GetByUser(string userId) =>
        await db.Notifications
            .Include(n => n.Booking)
            .Where(n => n.Booking.BookedBy == userId)
            .OrderByDescending(n => n.SentAt)
            .ToListAsync();

    public async Task<bool> MarkRead(int id)
    {
        var n = await db.Notifications.FindAsync(id);
        if (n is null) return false;
        n.IsRead = true;
        await db.SaveChangesAsync();
        return true;
    }
}
