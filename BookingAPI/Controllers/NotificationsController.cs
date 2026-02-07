using Microsoft.AspNetCore.Mvc;
using BookingAPI.Services;

namespace BookingAPI.Controllers;

[ApiController, Route("api/[controller]")]
public class NotificationsController(NotificationService svc) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<ActionResult> GetByUser(string userId)
    {
        var list = await svc.GetByUser(userId);
        return Ok(list.Select(n => new
        {
            n.Id, n.BookingId, n.Message, n.SentAt, n.IsRead
        }));
    }

    [HttpPut("{id}/read")]
    public async Task<ActionResult> MarkRead(int id) =>
        await svc.MarkRead(id) ? NoContent() : NotFound();
}
