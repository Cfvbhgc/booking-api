namespace BookingAPI.DTOs;

public record CreateRoomRequest(string Name, int Capacity, string Location);
