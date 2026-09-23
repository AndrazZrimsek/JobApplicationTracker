# Job Application Tracker API

A REST API for managing job applications, built with **C#, ASP.NET Core (.NET 10), Entity Framework Core, and SQLite**.

I built this project to deepen my hands-on experience with modern ASP.NET Core while applying my existing C#/.NET and backend development experience. It focuses on a maintainable controller–service architecture, dependency injection, database persistence, and practical API concerns such as validation, pagination, logging, and error handling.

> **Status:** Functional backend API; automated tests and authentication are planned enhancements.

## Features

- Create, retrieve, update, and delete job applications through REST endpoints.
- Store applications in SQLite with Entity Framework Core and version the schema through migrations.
- Use asynchronous controller, service, and database operations.
- Keep HTTP concerns in controllers and data-access logic in an injectable service (`IApplicationService` / `ApplicationService`).
- Use separate request DTOs for creating and updating applications, with attribute-based validation and automatic `400 Bad Request` responses.
- Filter applications by status or company and retrieve paginated results, including the total number of matching records.
- Record application events and failed lookups using structured `ILogger<T>` logging.
- Handle unhandled exceptions centrally and return standardized Problem Details responses.
- Generate an OpenAPI document in the Development environment.

## Tech stack

| Area | Technology |
| --- | --- |
| Language and framework | C#, .NET 10, ASP.NET Core Web API |
| API style | Attribute-routed controllers |
| Dependency injection | Built-in ASP.NET Core DI |
| Persistence | Entity Framework Core, SQLite |
| Validation | Data annotations and `[ApiController]` |
| Observability | `ILogger<T>`, Problem Details |
| API specification | ASP.NET Core OpenAPI |

## Architecture

```text
HTTP request
    |
    v
ApplicationsController   <-- request DTOs, validation, HTTP responses
    |
    v
IApplicationService
    |
    v
ApplicationService      <-- application logic, filtering, logging
    |
    v
ApplicationDbContext    <-- EF Core
    |
    v
SQLite
```

`ApplicationService` and `ApplicationDbContext` are registered with scoped lifetimes. Controllers depend on `IApplicationService`, allowing the service implementation to be replaced independently. EF Core tracks changes to entities and persists them with `SaveChangesAsync()`.

## API endpoints

| Method | Endpoint | Description | Success response |
| --- | --- | --- | --- |
| `GET` | `/api/applications` | List, filter, and paginate applications | `200 OK` |
| `GET` | `/api/applications/{id}` | Get one application | `200 OK` |
| `POST` | `/api/applications` | Create an application | `201 Created` |
| `PUT` | `/api/applications/{id}` | Update an application | `204 No Content` |
| `DELETE` | `/api/applications/{id}` | Delete an application | `204 No Content` |

Requests for nonexistent IDs return `404 Not Found`. Invalid create/update requests return `400 Bad Request` with validation details.

### Create an application

`POST /api/applications`

```json
{
  "company": "Example Corp",
  "position": "Software Engineer",
  "status": "Applied",
  "appliedDate": "2026-09-16",
  "notes": "Applied through the company website"
}
```

The database generates the application's ID. A successful response includes the created application and a `Location` header pointing to `/api/applications/{id}`.

### Filter and paginate

For example:

```http
GET /api/applications?status=Interview&company=Example&page=1&pageSize=10
```

Both filters are optional. Status is matched exactly; company supports substring matching. Results are ordered by application date (newest first), then paginated. The response has this shape:

```json
{
  "items": [
    {
      "id": 2,
      "company": "Example Corp",
      "position": "Software Engineer",
      "status": "Interview",
      "appliedDate": "2026-09-16",
      "notes": "Interview scheduled"
    }
  ],
  "totalCount": 1
}
```

`totalCount` is calculated *before* pagination, so it represents all records matching the filters rather than just the current page. The query DTO supplies default page settings and validates pagination inputs.

## Run locally

**Prerequisites:** [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) and the EF Core CLI tool (`dotnet-ef`).

1. Clone the repository and open its project directory.
2. Restore dependencies:

   ```bash
   dotnet restore
   ```

3. Check the SQLite connection string under `ConnectionStrings` in `appsettings.json`. The project uses a local SQLite database; database files are excluded from Git.
4. If you don't have the EF Core CLI tool installed, install it:

   ```bash
   dotnet tool install --global dotnet-ef
   ```

5. Apply the committed migrations to create the local database:

   ```bash
   dotnet ef database update
   ```

6. Run the API:

   ```bash
   dotnet run
   ```

Use the URL shown in the terminal (the development launch profile currently uses `http://localhost:5057`). With the app running in Development, its generated OpenAPI document is available at `/openapi/v1.json`.

The repository includes EF Core migrations but does **not** include a populated SQLite database. Running the migration command creates your own local schema.

## Roadmap

The current API is a functional backend portfolio project. Potential next steps:

- [ ] Add service unit tests and HTTP integration tests.
- [ ] Add authentication and authorization, with applications scoped to their owner.
- [ ] Improve OpenAPI endpoint descriptions and add more pagination metadata.
- [ ] Optionally add a small frontend for interacting with the API.

**Note:** Authentication is not implemented yet. This version is intended for local development and demonstration, not for storing private job-search data on a publicly exposed server.
