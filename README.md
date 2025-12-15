# Loan Management System - Take Home Test
Full-stack take-home implementation: .NET 6 Web API + SQL Server (Docker) + Angular.

## Tech Stack
- Backend: .NET 6 (ASP.NET Core), EF Core 6 (Code First + Migrations), SQL Server (Docker)
- Frontend: Angular (standalone components), Angular Material, dev proxy to avoid CORS
- Testing: xUnit; unit tests with EF Core InMemory; integration tests with WebApplicationFactory

## How to Run (Local)
### Prerequisites
- .NET SDK 6.x
- Docker Desktop
- Node.js + npm (for Angular)

### 1) Database (SQL Server via Docker)
```bash
docker compose up -d sqlserver
```
Provisioned at `localhost:1433` (Apple Silicon runs under `linux/amd64` emulation).

### 2) Backend
Restore dependencies:
```bash
dotnet restore backend/src/src.sln
```
Apply migrations:
```bash
cd backend/src
dotnet ef database update --project Fundo.Applications.WebApi
```
Run API:
```bash
cd backend/src/Fundo.Applications.WebApi
dotnet run
```
API available at `http://localhost:5000`.

### 3) API Endpoints
- GET `/loans`
- GET `/loans/{id}`
- POST `/loans`
- POST `/loans/{id}/payment`

Example:
```bash
curl http://localhost:5000/loans
```

### 4) Seed Data
On startup, migrations run and sample data is inserted only when the database is empty.

### 5) Frontend
```bash
cd frontend
npm install
npm start
```
Runs on `http://localhost:4200`.

**Proxy (no CORS):** `proxy.conf.json` forwards `/api/*` to the backend.
```bash
curl http://localhost:4200/api/loans
```

## Running Tests
```bash
dotnet test backend/src/src.sln
```
- Unit tests: EF Core InMemory
- Integration tests: WebApplicationFactory + SQL Server

## GitHub Actions CI
- Restores
- Builds
- Runs unit tests only (integration tests skipped to avoid provisioning SQL Server in CI)

## Implementation Log
- Day 1: Domain + EF Core setup; Dockerized SQL Server; Code First migrations.
- Day 2: API endpoints; seed data initializer; unit + integration tests; Angular integration; CORS-free proxy; CI pipeline.

## Final Status
Fully runnable locally, tested, documented, and CI-enabled for backend validation.

---

## Improvements / Next Steps (Not Implemented)

### Authentication & Authorization
For the scope and time constraints of this take-home, I did not implement authentication.

Given more time, I would add:
- Secure secrets management (environment variables + secrets store in CI/CD).

### Frontend Testing (Angular)
Due to time constraints, I did not implement automated tests for the Angular frontend.

With additional time, I would add:
- **Unit tests for components** focusing on:
  - Data rendering (loan list, status labels, formatting).
  - Interaction with services.
- **Service tests** for `LoanService` using `HttpClientTestingModule` to mock backend API calls.
- Basic **error handling tests** to ensure the UI behaves correctly on API failures.


## Implementation Approach (Day 1)
The first day was focused on establishing a solid and reproducible backend foundation before implementing business logic or UI features.

The main goals for Day 1 were:
- Define the core domain model
- Configure persistence using Entity Framework Core (Code First)
- Set up SQL Server locally using Docker
- Ensure database schema creation via migrations
- Keep the system runnable and consistent at every step

### Backend Setup
- Implemented a clean `Loan` domain entity with a strongly typed `LoanStatus` enum.
- Configured Entity Framework Core with SQL Server using a dedicated `AppDbContext`.
- Used Code First migrations to version and manage the database schema.
- Externalized configuration using `appsettings.json` and `appsettings.Development.json`.

### Infrastructure
- Added a `docker-compose.yml` file at the repository root to provision SQL Server.
- Used Docker to ensure a consistent local development environment without relying on local SQL Server installations.
- Successfully created the database and tables via EF Core migrations.

---

## Challenges Faced & Solutions

During Day 1, several real-world configuration and tooling issues were encountered and resolved:

### EF Core and .NET Version Compatibility
- **Issue:** EF Core 10 was initially installed automatically, causing incompatibility with the project’s `net6.0` target.
- **Solution:** Downgraded EF Core and `dotnet-ef` to version 6.x to match the framework version.

### Configuration Injection in Startup
- **Issue:** EF Core migrations failed because `IConfiguration` was injected into `Startup` but not stored, resulting in a null connection string at design time.
- **Solution:** Properly assigned the injected configuration to a class-level property in `Startup`, enabling EF Core to resolve `ConnectionStrings` correctly.

### AppSettings Naming Issues
- **Issue:** Migrations failed due to a typo in the `appsettings.Development.json` filename.
- **Solution:** Corrected the filename to match ASP.NET Core configuration conventions.

### SQL Server Authentication in Docker
- **Issue:** SQL Server login failed for user `sa` due to password mismatch.
- **Cause:** Docker containers persist credentials after the first initialization.
- **Solution:** Restarted Docker and recreated the SQL Server container to ensure credentials matched the configured connection string.

### Apple Silicon (ARM) Compatibility
- **Issue:** Docker emitted warnings about running an `amd64` SQL Server image on an `arm64` host.
- **Solution:** Verified that SQL Server ran correctly under emulation and continued, as performance was sufficient for development and the challenge scope.

---

## Current Status (End of Day 1)

By the end of Day 1:
- The backend API is correctly configured with .NET 6 and EF Core.
- SQL Server is running in Docker.
- The database schema has been created via migrations.
- The system is ready for seed data, API endpoints, tests, and frontend integration.

---

## Implementation Approach (Day 2)

The second day focused on delivering end-to-end functionality: implementing business logic, exposing endpoints, validating behavior through tests, and integrating the backend with a Angular frontend.

The main goals for Day 2 were:
- Implement the complete Loan Management API
- Apply business rules in a dedicated service layer
- Validate behavior through unit and integration tests
- Integrate the backend with an Angular frontend
- Keep the solution simple, readable, and aligned with the challenge scope

---

### Backend – Business Logic & API Endpoints

- Implemented all required RESTful endpoints:
  - `GET /loans` – list all loans
  - `GET /loans/{id}` – retrieve loan details
  - `POST /loans` – create a new loan
  - `POST /loans/{id}/payment` – register a loan payment
- Introduced a `LoanService` to encapsulate business logic and keep controllers thin.
- Applied domain rules such as:
  - Initial balance equals loan amount
  - Preventing overpayments
  - Marking loans as `Paid` when the balance reaches zero
- Ensured proper HTTP semantics (e.g., `201 Created` with `Location` header when creating resources).

---

### Seed Data

- Added database seed logic executed at application startup.
- The seed checks for existing data before inserting records, ensuring:
  - Data persistence across restarts
  - Idempotent initialization
- This allows the frontend to consume meaningful data without manual database setup.

---

### Testing Strategy

- Implemented **unit tests** for the `LoanService` using **xUnit** and **EF Core InMemory**:
  - Validated core business rules (loan creation, payments, edge cases).
- Implemented **integration tests** using `WebApplicationFactory`:
  - Verified API routing, serialization, status codes.
  - Detected and fixed an async bug (`Task` being returned instead of awaited entity).
- The combination of unit and integration tests ensures both correctness of logic and API stability.

---

### Frontend – Angular Integration

- Integrated a Angular frontend.
- Configured an Angular development proxy to avoid CORS issues without modifying backend configuration.
- Replaced hardcoded UI data with live data fetched from the backend API.
- Implemented a Material table to display loans with real-time values from the API.
- Kept frontend logic intentionally simple to focus on backend integration and correctness.

---

## Challenges Faced & Solutions (Day 2)

### Async Handling in Controllers
- **Issue:** A controller method returned a `Task` instead of the awaited result, causing JSON serialization errors during integration testing.
- **Solution:** Properly awaited asynchronous service calls, fixing both runtime behavior and test failures.

### Angular CORS & API Integration
- **Issue:** Browser-level CORS restrictions when calling the backend from Angular.
- **Solution:** Configured Angular’s development proxy to transparently forward API requests to the backend.

### Frontend–Backend Contract Alignment
- **Issue:** Initial mismatch between frontend field names and backend DTOs.
- **Solution:** Aligned the frontend model with the backend response to keep the API contract explicit and consistent.

---

## Current Status (End of Day 2)

By the end of Day 2:
- All backend endpoints are fully implemented and tested.
- Business logic is encapsulated in a dedicated service layer.
- Unit and integration tests validate both logic and API behavior.
- The Angular frontend successfully consumes live data from the backend.
