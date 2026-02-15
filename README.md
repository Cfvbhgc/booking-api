# BookingAPI

REST API for room and hall booking management.

## Features
- CRUD operations for rooms
- Create and cancel bookings with conflict detection
- Time slot management
- Notification system
- Swagger UI documentation

## Tech Stack
- ASP.NET Core 8 Web API
- Entity Framework Core + PostgreSQL
- Swashbuckle (Swagger)

## Setup
```bash
# Update connection string in appsettings.json
cd BookingAPI
dotnet restore
dotnet ef database update
dotnet run
```
Swagger UI: https://localhost:5001/swagger

## API Endpoints
| Method | Endpoint | Description |
|--------|---------|-------------|
| GET | /api/rooms | List all rooms |
| POST | /api/rooms | Create room |
| GET | /api/rooms/{id}/availability | Check availability |
| POST | /api/bookings | Create booking |
| PUT | /api/bookings/{id}/cancel | Cancel booking |
| GET | /api/notifications/{userId} | Get notifications |
