# **Take-Home Test: Backend-Focused Full-Stack Developer (.NET C# & Angular)**

## **Objective**

This take-home test evaluates your ability to develop and integrate a .NET Core (C#) backend with an Angular frontend, focusing on API design, database integration, and basic DevOps practices.

## **Instructions**

1.  **Fork the provided repository** before starting the implementation.
2.  Implement the requested features in your forked repository.
3.  Once you have completed the implementation, **send the link** to your forked repository via email for review.

## **Task**

You will build a simple **Loan Management System** with a **.NET Core backend (C#)** exposing RESTful APIs and a **basic Angular frontend** consuming these APIs.

---

## **Requirements**

### **1. Backend (API) - .NET Core**

* Create a **RESTful API** in .NET Core to handle **loan applications**.
* Implement the following endpoints:
    * `POST /loans` → Create a new loan.
    * `GET /loans/{id}` → Retrieve loan details.
    * `GET /loans` → List all loans.
    * `POST /loans/{id}/payment` → Deduct from `currentBalance`.
* Loan example (feel free to improve it):

    ```json
    {
        "amount": 1500.00, // Amount requested
        "currentBalance": 500.00, // Remaining balance
        "applicantName": "Maria Silva", // User name
        "status": "active" // Status can be active or paid
    }
    ```

* Use **Entity Framework Core** with **SQL Server**.
* Create seed data to populate the loans (the frontend will consume this).
* Write **unit/integration tests for the API** (xUnit or NUnit).
* **Dockerize** the backend and create a **Docker Compose** file.
* Create a README with setup instructions.

### **2. Frontend - Angular (Simplified UI)**  

Develop a **lightweight Angular app** to interact with the backend

#### **Features:**  
- A **table** to display a list of existing loans.  

#### **Mockup:**  
[View Mockup](https://kzmgtjqt0vx63yji8h9l.lite.vusercontent.net/)  
(*The design doesn’t need to be an exact replica of the mockup—it serves as a reference. Aim to keep it as close as possible.*)  

---

## **Bonus (Optional, Not Required)**

* **Improve error handling and logging** with structured logs.
* Implement **authentication**.
* Create a **GitHub Actions** pipeline for building and testing the backend.

---

## **Evaluation Criteria**

✔ **Code quality** (clean architecture, modularization, best practices).

✔ **Functionality** (the API and frontend should work as expected).

✔ **Security considerations** (authentication, validation, secure API handling).

✔ **Testing coverage** (unit tests for critical backend functions).

✔ **Basic DevOps implementation** (Docker for backend).

---

## **Additional Information**

Candidates are encouraged to include a `README.md` file in their repository detailing their implementation approach, any challenges they faced, features they couldn't complete, and any improvements they would make given more time. Ideally, the implementation should be completed within **two days** of starting the test.

---

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
