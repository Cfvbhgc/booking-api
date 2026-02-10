namespace BookingAPI.DTOs;

public record CreateBookingRequest(int RoomId, string Title, string BookedBy, DateTime StartTime, DateTime EndTime);
