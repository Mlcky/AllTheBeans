# AllTheBeans

## Instructions to Run

### Prerequisites

.NET 8 SDK installed

### Running the API

1. Clone the repository.

2. Navigate to the API project folder:

```
cd src/AllTheBeans.Api
```

3. Run the application:

```
dotnet run
```

The application will automatically create the SQLite database, apply migrations, and seed initial data.

4. Access the API at:

Swagger UI: https://localhost:5001/swagger

### Running Tests

From the root of the solution:

```
dotnet test
```

This runs all unit and integration tests.
Integration tests use an in-memory SQLite database, so they run without extra setup.
