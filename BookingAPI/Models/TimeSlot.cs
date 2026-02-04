namespace BookingAPI.Models;

public class TimeSlot
{
    public int Id { get; set; }
    public int RoomId { get; set; }
    public int DayOfWeek { get; set; }
    public int StartHour { get; set; }
    public int EndHour { get; set; }
}
