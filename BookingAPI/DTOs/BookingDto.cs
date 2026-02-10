namespace BookingAPI.DTOs;

public record BookingDto(int Id, int RoomId, string RoomName, string Title, string BookedBy,
    DateTime StartTime, DateTime EndTime, string Status, DateTime CreatedAt);
