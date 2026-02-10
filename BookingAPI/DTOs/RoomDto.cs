namespace BookingAPI.DTOs;

public record RoomDto(int Id, string Name, int Capacity, string Location, bool IsActive);
