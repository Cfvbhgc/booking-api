using Microsoft.AspNetCore.Mvc;
using BookingAPI.DTOs;
using BookingAPI.Services;

namespace BookingAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class BookingsController(BookingService svc) : ControllerBase
{
    static BookingDto ToDto(Models.Booking b) =>
        new(b.Id, b.RoomId, b.Room?.Name ?? "", b.Title, b.BookedBy,
            b.StartTime, b.EndTime, b.Status.ToString(), b.CreatedAt);

    [HttpGet]
    public async Task<ActionResult<List<BookingDto>>> GetAll() =>
        (await svc.GetAll()).Select(ToDto).ToList();

    [HttpGet("{id}")]
    public async Task<ActionResult<BookingDto>> Get(int id)
    {
        var b = await svc.GetById(id);
        return b is null ? NotFound() : ToDto(b);
    }

    [HttpPost]
    public async Task<ActionResult<BookingDto>> Create(CreateBookingRequest req)
    {
        var b = await svc.Create(req);
        b = await svc.GetById(b.Id);
        return CreatedAtAction(nameof(Get), new { id = b!.Id }, ToDto(b));
    }

    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<BookingDto>> Cancel(int id)
    {
        var b = await svc.Cancel(id);
        if (b is null) return NotFound();
        b = await svc.GetById(b.Id);
        return ToDto(b!);
    }

    [HttpGet("room/{roomId}")]
    public async Task<ActionResult<List<BookingDto>>> GetByRoom(int roomId) =>
        (await svc.GetByRoom(roomId)).Select(ToDto).ToList();
}
