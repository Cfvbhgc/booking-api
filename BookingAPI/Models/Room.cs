namespace BookingAPI.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Capacity { get; set; }
    public string Location { get; set; } = "";
    public bool IsActive { get; set; } = true;
    public List<Booking> Bookings { get; set; } = [];
    public List<TimeSlot> TimeSlots { get; set; } = [];
}
