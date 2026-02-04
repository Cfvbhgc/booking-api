namespace BookingAPI.Models;

public enum BookingStatus { Pending, Confirmed, Cancelled }

public class Booking
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; } = null!;
    public string Title { get; set; } = "";
    public string BookedBy { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Notification> Notifications { get; set; } = [];
}
