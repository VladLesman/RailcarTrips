Railcar Trips – Assessment Submission

This solution implements a Blazor WebAssembly application backed by an ASP.NET Core API and an EF Core In‑Memory database.

The goal is to ingest equipment event data from CSV files, convert those events into railcar trips, and present the results in a clean UI.

The implementation focuses on clarity, separation of concerns, and demonstrating architectural thinking rather than delivering a fully production‑ready system.

Solution Architecture

The solution follows a layered architecture with clear boundaries between UI, API, application logic, domain logic, and infrastructure.

Layer Overview

RailcarTrips (Blazor WebAssembly UI)
Provides the user interface for:
    • Uploading cities CSV
    • Uploading equipment events CSV
    • Viewing processed trips
    • Viewing trip event details
Uses MudBlazor for layout and styling. Communicates with the API via typed HttpClient services.

RailcarTrips.Api (Web API)
Exposes endpoints for:
    • Uploading CSV files
    • Querying processed trips
Delegates all business logic to the Application layer.

RailcarTrips.Application (Use Case Layer)
Implements application workflows:
    • Importing cities
    • Importing equipment events
    • Processing trips
    • Querying trips
Contains no domain logic; orchestrates domain and infrastructure operations.

RailcarTrips.Core (Domain Layer)
Contains pure business logic:
    • TripProcessor (W → Z trip construction)
    • Domain entities
    • Domain contracts
No dependencies on infrastructure or UI.

RailcarTrips.Infrastructure (Infrastructure Layer)
Implements:
    • EF Core InMemory database
    • Repositories
    • CSV parsing (CsvHelper)
    • Time zone conversion utilities
Bridges domain and application layers.

RailcarTrips.Shared.Dtos
Defines DTOs shared between client and server.

Tests
Unit tests for domain logic (TripProcessor).

Trip Processing Logic
Trips are constructed per equipment using the following rules:
    • W (Released) → start a new trip
    • Z (Placed) → end the current trip
    • Events between W and Z belong to the trip
    • Trips are sorted by UTC time
    • Total trip duration = (EndUtc - StartUtc).TotalHours
Event timestamps are converted from local time to UTC using the city’s time zone.

Assumptions
    1. City time zones in the CSV are valid Windows time zone IDs.
    2. Event timestamps are local to the city and require conversion to UTC.
    3. Trips always follow a W → Z pattern.
    4. Incomplete trips (W without Z) are ignored for now.
    5. Duplicate uploads are not deduplicated (TODO).
    6. InMemory database is acceptable for assessment purposes.
    
Questions for Clarification
    1. Should incomplete trips be stored or discarded?
    2. Should multiple consecutive W events close the previous trip?
    3. Should the UI show local times, UTC times, or both?
    4. Should re-uploading CSVs clear existing data?
    5. Should trip processing be idempotent?
    
TODO / Future Improvements
    • Add validation for malformed CSV rows.
    • Add logging for inconsistent event sequences.
    • Add paging, sorting, and filtering to the trips grid.
    • Add persistent storage (SQL Server).
    • Add more unit tests for edge cases.
    • Add error details to the UI instead of generic messages.
    • Add authentication if required.
    
Running the Solution
    1. Open the solution in Visual Studio or Rider.
    2. Set RailcarTrips.Api as the startup project.
    3. Run the API.
    4. Run RailcarTrips (Blazor WASM).
    5. Navigate to:
        ◦ /upload-cities to upload city data
        ◦ /upload to upload equipment events
        ◦ / to view processed trips
