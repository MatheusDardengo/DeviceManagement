# Device Management API

Simple ASP.NET Core Web API for device lifecycle management, following a clean layered structure following DDD and Clean Architecture principles.

## Tech Stack

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core (SQL Server)
- FluentValidation
- xUnit + Moq + FluentAssertions
- Docker & Docker Compose (Containerization)

## Solution Structure

- `DeviceManagement.Api` → HTTP layer (controllers, middleware, filters)
- `DeviceManagement.Application` → use cases, DTOs, service logic
- `DeviceManagement.Domain` → entities and business rules
- `DeviceManagement.Infrastructure` → EF Core context and repositories
- `DeviceManagement.Test` → unit tests

## Features

- Create device
- Get device by id
- Get all devices (optional `brand` and `state` filters)
- Update device
- Delete device

## API Endpoints

Base route: `/Devices`

- `POST /Devices`  
  Creates a new device.

- `GET /Devices?id={guid}`  
  Returns one device by id.

- `GET /Devices/GetAll?brand={brand}&state={state}`  
  Returns all devices with optional filters.

- `PUT /Devices/{id}`  
  Updates name/brand/state of an existing device.

- `DELETE /Devices/{id}`  
  Deletes a device.

## Local Setup

1. Update connection string in `appsettings.json` (`DefaultConnection`).
2. Run the API project.
3. EF migrations are applied on startup (`Database.Migrate()` in `Program.cs`).

Optional: docs/insomnia_collection.json file with pre-configured requests for testing the API. 

## Current Conventions

- Identifier naming rules reference:  
  https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names

---

## Next Steps / Improvements

### 1) Pagination
- Add pagination to `GET /Devices/GetAll`
- Suggested query params: `pageNumber`, `pageSize`
- Return metadata (total records, total pages, current page)

### 2) Logging
- Add structured logging across API/Application layers
- Log request lifecycle, validation failures, and exceptions
- Correlate logs with trace/request id for troubleshooting

### 3) Authentication & Authorization
- Add JWT Bearer authentication with Acess Tokens, Refresh Tokens (with token rotation)
- Protect write operations (`POST`, `PUT`, `DELETE`) with roles/policies
- Keep `GET` endpoints public or policy-based as needed
- Make a stronger indentifier for devices (e.g., IMEI) and enforce uniqueness