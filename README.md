# .NET API Starter

> The `SampleItem` feature is intentionally included as an end-to-end reference and can be removed when starting a real project.

A reusable, production-oriented starter project for building modern .NET APIs.

The project provides a ready-to-use foundation based on .NET, Clean Architecture and Vertical Slice Architecture, with authentication, persistence, caching, observability, automated testing and containerized local development already configured.

It is intended to reduce the amount of infrastructure and cross-cutting setup required when starting a new backend project.

## Project goals

The starter aims to provide a practical foundation for .NET backend applications with:

- Clean Architecture dependency boundaries
- Vertical Slice Architecture for application use cases
- ASP.NET Core Minimal APIs
- Entity Framework Core with PostgreSQL
- Redis distributed caching
- HybridCache with local and distributed caching
- OAuth 2.0 and OpenID Connect authentication with Keycloak
- Role-based and policy-based authorization
- Consistent Problem Details error responses
- OpenTelemetry observability
- Structured logging
- Distributed tracing
- Runtime and HTTP metrics
- Seq integration
- .NET Aspire local orchestration
- Docker Compose support
- Automated unit, integration and architecture tests
- Testcontainers and Respawn
- Static analysis with SonarQube Cloud
- GitHub Actions CI
- Container image publication to GitHub Container Registry
- Shared build configuration through `Directory.Build.props`
- NuGet Central Package Management
- Shared MSBuild configuration
- EditorConfig through `Directory.Packages.props`

The project intentionally avoids introducing abstractions that do not provide a concrete benefit.

## Architecture

The solution is structured as a modular monolith using Clean Architecture principles combined with Vertical Slice Architecture.

```text
src/
├── Starter.Api
├── Starter.Application
├── Starter.Domain
├── Starter.Infrastructure
├── Starter.AppHost
└── Starter.ServiceDefaults

tests/
├── Starter.Domain.Tests
├── Starter.Application.Tests
├── Starter.Api.Tests
└── Starter.Architecture.Tests
```

Clean Architecture defines the dependency boundaries between the main projects, while Vertical Slice Architecture organizes application code around individual use cases.

The core dependency direction is:

```text
Starter.Api ------------> Starter.Application
Starter.Api ------------> Starter.Infrastructure

Starter.Infrastructure -> Starter.Application
Starter.Application ----> Starter.Domain
```

The following dependencies are intentionally prevented:

```text
Starter.Domain ---------> Starter.Application
Starter.Domain ---------> Starter.Infrastructure
Starter.Application ----> Starter.Infrastructure
```

Architecture tests automatically enforce important dependency and structural rules.

## Example feature

The repository includes a small `SampleItem` feature that demonstrates how a complete vertical slice can be implemented.

The example includes:

```text
SampleItem
├── Domain model
├── Strongly typed identifier
├── Value object
├── Create command
├── Get query
├── EF Core persistence
├── HybridCache
├── Redis distributed cache
├── Authorization
├── Structured logging
└── Automated tests
```

Available endpoints:

```http
POST /api/sample-items
GET  /api/sample-items/{id}
```

The example feature is intentionally simple and can be removed when starting a real project.

Its purpose is to provide an executable reference for implementing new features.

## Application layer

Application code is organized as vertical slices.

For example:

```text
Starter.Application/
├── Common/
│   ├── CQRS/
│   ├── Caching/
│   └── Persistence/
│
└── SampleItems/
    ├── Create/
    └── GetById/
```

Commands and queries are implemented directly without MediatR.

Entity Framework Core is also used directly by application handlers instead of being hidden behind generic repository or unit of work abstractions.

This allows individual use cases to use:

- LINQ queries
- Projections
- Change tracking
- Transactions
- `AsNoTracking()`
- EF Core-specific optimizations

without introducing unnecessary indirection.

## Persistence

PostgreSQL is used as the relational database.

Entity Framework Core provides:

- Database mappings
- Change tracking
- LINQ query composition
- Transactions
- Schema migrations
- PostgreSQL persistence

The `DbContext` belongs to the Application layer and is consumed directly by application handlers.

Generic repository and unit of work abstractions are intentionally not used because Entity Framework Core already provides those capabilities.

## Caching

The starter uses `HybridCache` for cached application reads.

The cache architecture is:

```text
Application
    |
    v
HybridCache
   /     \
  /       \
L1         L2
Memory     Redis
  \         /
   \       /
   PostgreSQL
```

The local memory cache provides fast access within an individual API instance.

Redis acts as the distributed secondary cache and can be shared by multiple application instances.

Redis is configured through the .NET Aspire Redis distributed cache integration, which also provides telemetry for Redis operations.

Cache entries remain disposable and can always be rebuilt from PostgreSQL.

## Authentication and authorization

Keycloak is used as the external identity provider.

Authentication is based on:

- OAuth 2.0
- OpenID Connect
- JWT Bearer tokens

The example configuration includes the following application roles:

```text
Admin
User
```

The example create endpoint demonstrates policy-based authorization.

Keycloak uses:

```text
Realm: Starter
Client: Starter-api
```

The realm configuration is versioned in the repository and packaged into a dedicated Keycloak container image.

Generated signing and encryption key material is not stored in source control.

## Error handling

Expected application failures are represented using a Result pattern.

Errors can represent categories such as:

```text
Validation
Not Found
Conflict
```

The API translates application failures into consistent RFC 7807 Problem Details responses.

Domain invariant violations are represented by domain exceptions and handled centrally.

Unexpected exceptions are converted into generic `500 Internal Server Error` responses without exposing internal implementation details.

## Observability

OpenTelemetry is configured centrally in `Starter.ServiceDefaults`.

The current telemetry setup includes:

### Logging

Application logging uses the standard `Microsoft.Extensions.Logging` abstractions.

Structured message templates are preferred:

```csharp
logger.LogWarning(
    "Sample item with code {Code} already exists",
    code.Value);
```

Properties such as `Code` remain independently searchable in observability backends.

Serilog is not required by the current setup.

### Tracing

Distributed tracing currently covers:

- ASP.NET Core requests
- outgoing `HttpClient` calls
- Entity Framework Core database operations
- PostgreSQL queries
- Redis operations

This makes it possible to follow a request across the API, cache and database layers.

### Metrics

Metrics include:

- ASP.NET Core metrics
- `HttpClient` metrics
- .NET runtime metrics

Metrics are available through the Aspire Dashboard.

### Seq

Seq is integrated as an additional OpenTelemetry backend for:

- structured logs
- distributed traces

Seq is useful for searching structured properties and correlating application events with traces.

## .NET Aspire

`Starter.AppHost` orchestrates the local development environment.

It coordinates:

```text
Starter API
    |
    +---- PostgreSQL
    |
    +---- Redis
    |
    +---- Keycloak
    |
    +---- Seq
    |
    +---- EF Core migrations
```

Aspire provides:

- service discovery
- connection string injection
- resource dependencies
- health checks
- logs
- traces
- metrics
- local resource management

The Aspire Dashboard provides a single place to inspect the complete application topology.

## Docker Compose

The project also provides a Docker Compose environment that can run independently from .NET Aspire.

The Compose environment includes:

- PostgreSQL
- Redis
- Keycloak
- Seq
- database migrations
- Starter API

Database migrations run in a dedicated one-shot container before the API starts.

The Keycloak image contains the versioned Starter realm configuration.

Container images can be published to GitHub Container Registry.

## Testing strategy

Automated testing is divided into separate projects according to responsibility.

### Domain tests

`Starter.Domain.Tests` contains fast unit tests for domain behaviour and invariants.

These tests have no database or external infrastructure dependencies.

### Application integration tests

`Starter.Application.Tests` verifies application handlers and persistence against a real PostgreSQL instance.

The tests use:

- xUnit
- Testcontainers
- PostgreSQL
- Entity Framework Core migrations
- Respawn

Entity Framework Core is not mocked.

### API integration tests

`Starter.Api.Tests` verifies the application through its HTTP boundary.

The tests cover:

- HTTP endpoints
- request and response contracts
- Problem Details responses
- authentication
- authorization policies
- persistence
- cache behaviour

Authentication is replaced by a dedicated test authentication scheme so that application authorization can be tested independently from Keycloak.

### Architecture tests

`Starter.Architecture.Tests` automatically verifies architectural rules such as:

- project dependency boundaries
- Domain independence
- namespace conventions
- structural conventions

This makes architectural constraints executable rather than relying only on documentation.

## Code quality

The project uses SonarQube Cloud for static analysis.

The GitHub Actions CI pipeline:

```text
Restore
   |
Build
   |
Tests
   |
Coverage
   |
SonarQube analysis
   |
Quality Gate
   |
Docker image build
   |
GitHub Container Registry
```

Container image publication only happens after the build, tests and SonarQube Quality Gate succeed.

Compiler warnings are treated as errors.

Test coverage is collected automatically and reported to SonarQube Cloud.

Generated and orchestration-focused code such as migrations, AppHost and ServiceDefaults can be excluded from coverage metrics.

## Shared build, package and code style configuration

The solution centralizes common build settings and NuGet package versions at repository level.

### Directory.Build.props

`Directory.Build.props` defines shared MSBuild configuration that is applied automatically to projects in the solution.

It is used for cross-project build conventions such as:

- compiler settings
- nullable reference types
- implicit usings
- warnings treated as errors
- common analysis and quality rules

This avoids duplicating the same configuration across individual project files and helps keep build behaviour consistent throughout the solution.

### Directory.Packages.props

NuGet package versions are managed centrally through `Directory.Packages.props` using NuGet Central Package Management.

Individual `.csproj` files declare only the packages they depend on:

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" />
```

while the corresponding version is defined once at repository level:

```xml
<PackageVersion Include="Microsoft.EntityFrameworkCore" Version="..." />
```

This keeps dependency versions consistent across projects and makes package upgrades easier to review and maintain.

### .editorconfig

The repository includes an `.editorconfig` file to keep formatting and code
style rules consistent across editors and IDEs.

It defines shared conventions for areas such as:

- indentation and whitespace
- C# language style
- naming conventions
- analyzer preferences
- formatting rules

This helps keep the codebase consistent independently of individual developer
environment settings and complements the build-time quality rules defined in
`Directory.Build.props`.

## Technology stack

The starter currently uses:

- .NET 10
- ASP.NET Core Minimal APIs
- Entity Framework Core
- PostgreSQL
- HybridCache
- Redis
- .NET Aspire
- Keycloak
- OAuth 2.0
- OpenID Connect
- JWT Bearer authentication
- OpenTelemetry
- Seq
- OpenAPI
- Scalar
- xUnit
- Testcontainers
- Respawn
- NetArchTest
- SonarQube Cloud
- GitHub Actions
- Docker
- Docker Compose
- GitHub Container Registry
- NuGet Central Package Management
- Shared MSBuild configuration
- EditorConfig

## Running with .NET Aspire

Start the complete local environment through the AppHost:

```bash
dotnet run --project src/Starter.AppHost
```

Aspire will start the required local resources and expose the Aspire Dashboard.

## Running with Docker Compose

Start the containerized environment with:

```bash
docker compose up -d
```

The default local endpoints are:

```text
API       http://localhost:8081
Keycloak  http://localhost:8080
Seq       http://localhost:5341
```

The PostgreSQL and Redis services are also exposed locally for development.

## Example API flow

After obtaining a valid access token, a sample item can be created with:

```http
POST /api/sample-items
```

Example request:

```json
{
  "name": "Sample Item",
  "code": "SAMPLE-001"
}
```

The created item can then be retrieved with:

```http
GET /api/sample-items/{id}
```

The GET use case demonstrates:

```text
HTTP request
    |
    v
HybridCache
    |
    +---- L1 memory
    |
    +---- Redis
    |
    +---- PostgreSQL on cache miss
```

## Using the starter for a new project

When starting a new application:

1. Rename the `Starter.*` projects and namespaces.
2. Replace the `SampleItem` example with the real domain model.
3. Update the database name and connection strings.
4. Rename the Keycloak realm and client.
5. Replace the sample authorization policies and roles.
6. Update Docker image names.
7. Configure the SonarQube project.
8. Configure repository secrets used by CI.
9. Update this README with the new project's domain and purpose.

The infrastructure and cross-cutting configuration can remain largely unchanged.

## Design principles

The starter follows a few deliberate principles:

- Prefer simple framework APIs over unnecessary wrappers.
- Introduce abstractions only when they provide a concrete benefit.
- Keep API endpoints thin.
- Keep business rules in the Domain where appropriate.
- Organize application behaviour around use cases.
- Test against real infrastructure when practical.
- Treat observability, security and automated testing as part of the application foundation rather than later additions.

## Project status

The starter currently provides a complete baseline for developing a modern .NET backend application.

It includes persistence, caching, authentication, authorization, observability, testing, CI and local/containerized orchestration.

The `SampleItem` feature serves only as a reference implementation and is intended to be replaced by the domain-specific features of applications created from this starter.
