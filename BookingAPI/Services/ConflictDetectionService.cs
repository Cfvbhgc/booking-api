using Microsoft.EntityFrameworkCore;
using BookingAPI.Data;
using BookingAPI.Models;

namespace BookingAPI.Services;

public class ConflictDetectionService(AppDbContext db)
{
    public async Task<bool> HasConflict(int roomId, DateTime start, DateTime end, int? excludeId = null)
    {
        var q = db.Bookings.Where(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            b.StartTime < end &&
            b.EndTime > start);

        if (excludeId.HasValue)
            q = q.Where(b => b.Id != excludeId.Value);

        return await q.AnyAsync();
    }

    public async Task<List<Booking>> GetConflicts(int roomId, DateTime start, DateTime end) =>
        await db.Bookings.Where(b =>
            b.RoomId == roomId &&
            b.Status != BookingStatus.Cancelled &&
            b.StartTime < end &&
            b.EndTime > start).ToListAsync();
}
