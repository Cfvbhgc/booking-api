using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookingAPI.Data;
using BookingAPI.DTOs;
using BookingAPI.Models;

namespace BookingAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class RoomsController(AppDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<RoomDto>>> GetAll() =>
        await db.Rooms.Select(r => new RoomDto(r.Id, r.Name, r.Capacity, r.Location, r.IsActive)).ToListAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<RoomDto>> Get(int id)
    {
        var r = await db.Rooms.FindAsync(id);
        return r is null ? NotFound() : new RoomDto(r.Id, r.Name, r.Capacity, r.Location, r.IsActive);
    }

    [HttpPost]
    public async Task<ActionResult<RoomDto>> Create(CreateRoomRequest req)
    {
        var r = new Room { Name = req.Name, Capacity = req.Capacity, Location = req.Location };
        db.Rooms.Add(r);
        await db.SaveChangesAsync();
        var dto = new RoomDto(r.Id, r.Name, r.Capacity, r.Location, r.IsActive);
        return CreatedAtAction(nameof(Get), new { id = r.Id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, CreateRoomRequest req)
    {
        var r = await db.Rooms.FindAsync(id);
        if (r is null) return NotFound();
        r.Name = req.Name;
        r.Capacity = req.Capacity;
        r.Location = req.Location;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var r = await db.Rooms.FindAsync(id);
        if (r is null) return NotFound();
        r.IsActive = false;
        await db.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("{id}/availability")]
    public async Task<ActionResult> GetAvailability(int id, [FromQuery] DateTime date)
    {
        var r = await db.Rooms.FindAsync(id);
        if (r is null) return NotFound();

        var dayStart = date.Date;
        var dayEnd = dayStart.AddDays(1);

        var booked = await db.Bookings
            .Where(b => b.RoomId == id && b.Status != BookingStatus.Cancelled &&
                        b.StartTime < dayEnd && b.EndTime > dayStart)
            .Select(b => new { b.StartTime, b.EndTime, b.Title, b.BookedBy })
            .ToListAsync();

        var slots = await db.TimeSlots
            .Where(s => s.RoomId == id && s.DayOfWeek == (int)date.DayOfWeek)
            .Select(s => new { s.StartHour, s.EndHour })
            .ToListAsync();

        return Ok(new { RoomId = id, Date = date.Date, BookedSlots = booked, AvailableHours = slots });
    }
}
