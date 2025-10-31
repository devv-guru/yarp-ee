# YARP-EE (YARP Extended Edition)

An extension of the YARP Reverse proxy with a Blazor WASM UI, and additional support for database persistence and changing configuration without server restarts.

## Architecture

YARP-EE follows a **Ports & Adapters (Hexagonal)** architecture:

- **Domain Layer** (`YarpEe.Domain`): Framework-agnostic entities and value objects
- **Application Layer** (`YarpEe.Application`): Use cases and ports (interfaces)
- **Adapters**:
  - **Persistence** (`YarpEe.Adapters.Persistence.Postgres`): PostgreSQL with EF Core
  - **Proxy** (`YarpEe.Adapters.Proxy.Yarp`): YARP dynamic configuration
  - **WebAPI** (`YarpEe.WebApi`): ASP.NET Core REST API

## Features

- ✅ **Dynamic Route Configuration**: Create, update, and delete routes at runtime
- ✅ **Dynamic Cluster Management**: Manage upstream clusters and destinations
- ✅ **PostgreSQL Persistence**: All configurations stored in PostgreSQL
- ✅ **Hot Reload**: Apply configuration changes without restarting the service
- ✅ **Health Checks**: Built-in liveness and readiness probes
- ✅ **Hexagonal Architecture**: Clean separation of concerns with ports and adapters

## Prerequisites

- .NET 9.0 SDK
- Docker & Docker Compose (for local development)
- PostgreSQL 16+ (if running without Docker)

## Quick Start with Docker

1. **Clone the repository**:
   ```bash
   git clone https://github.com/devv-guru/yarp-ee.git
   cd yarp-ee
   ```

2. **Start the services**:
   ```bash
   docker compose up -d
   ```

3. **Wait for services to be healthy**:
   ```bash
   docker compose ps
   ```

4. **The API will be available at**: `http://localhost:8080`

## API Endpoints

### Routes

- `GET /api/routes` - List all routes
- `POST /api/routes` - Create a new route
- `PUT /api/routes/{id}` - Update a route
- `DELETE /api/routes/{id}` - Delete a route

### Clusters

- `GET /api/clusters` - List all clusters
- `POST /api/clusters` - Create a new cluster with destinations
- `PUT /api/clusters/{id}` - Update a cluster
- `DELETE /api/clusters/{id}` - Delete a cluster

### Proxy Control

- `POST /api/proxy/reload` - Trigger a manual reload of YARP configuration

### Health Checks

- `GET /health` - Liveness probe (basic health check)
- `GET /health/ready` - Readiness probe (includes database connectivity)

## Example Usage

### 1. Create a Cluster

```bash
curl -X POST http://localhost:8080/api/clusters \
  -H "Content-Type: application/json" \
  -d '{
    "name": "backend-service",
    "loadBalancingPolicy": "RoundRobin",
    "destinations": [
      {
        "address": "http://localhost:5001",
        "healthPath": "/health"
      },
      {
        "address": "http://localhost:5002",
        "healthPath": "/health"
      }
    ]
  }'
```

### 2. Create a Host

First, you need to create a host (currently requires direct database access or will be added to API):

```sql
INSERT INTO hosts (id, name, base_url, created_utc, updated_utc)
VALUES (gen_random_uuid(), 'api.example.com', 'http://api.example.com', NOW(), NOW());
```

### 3. Create a Route

```bash
curl -X POST http://localhost:8080/api/routes \
  -H "Content-Type: application/json" \
  -d '{
    "hostId": "<host-id-from-step-2>",
    "clusterId": "<cluster-id-from-step-1>",
    "path": "/api/{**catch-all}",
    "order": 0,
    "enabled": true,
    "methods": ["GET", "POST", "PUT", "DELETE"]
  }'
```

### 4. Reload Proxy Configuration

```bash
curl -X POST http://localhost:8080/api/proxy/reload
```

## Local Development (without Docker)

### 1. Start PostgreSQL

```bash
docker run -d \
  --name yarpee-postgres \
  -e POSTGRES_USER=yarpee \
  -e POSTGRES_PASSWORD=yarpee \
  -e POSTGRES_DB=yarpee \
  -p 5432:5432 \
  postgres:16
```

### 2. Set Environment Variable

```bash
export YARPEE__DB__CONNECTIONSTRING="Host=localhost;Port=5432;Database=yarpee;Username=yarpee;Password=yarpee"
```

### 3. Run the API

```bash
cd src/YarpEe.WebApi
dotnet run
```

The API will be available at `http://localhost:5000` (or as configured).

## Database Migrations

The application automatically runs migrations on startup. To create new migrations:

```bash
cd src/YarpEe.Adapters.Persistence.Postgres
dotnet ef migrations add <MigrationName> --context YarpEeDbContext
```

## Running Tests

```bash
dotnet test
```

## Configuration

### Environment Variables

- `YARPEE__DB__CONNECTIONSTRING` - PostgreSQL connection string (required)
- `ASPNETCORE_ENVIRONMENT` - Environment name (Development, Production, etc.)
- `ASPNETCORE_URLS` - URLs to listen on (default: `http://+:8080`)

### appsettings.json

Additional configuration can be provided through `appsettings.json` in the WebApi project.

## Project Structure

```
yarp-ee/
├── src/
│   ├── YarpEe.Domain/                 # Domain entities and value objects
│   ├── YarpEe.Application/            # Use cases and ports (interfaces)
│   ├── YarpEe.Adapters.Persistence.Postgres/  # EF Core implementation
│   ├── YarpEe.Adapters.Proxy.Yarp/    # YARP configuration adapter
│   └── YarpEe.WebApi/                 # ASP.NET Core API
├── tests/
│   └── YarpEe.Application.Tests/      # Unit tests
├── docker-compose.yml                 # Docker Compose configuration
├── Dockerfile                         # API Docker image
└── README.md                          # This file
```

## Architecture Principles

### Ports & Adapters (Hexagonal Architecture)

1. **Domain Layer** is completely independent - no framework dependencies
2. **Application Layer** defines ports (interfaces) that adapters implement
3. **Adapters** depend inward on ports, never the other way around
4. **Dependency Injection** is configured in the WebApi composition root

### Key Design Decisions

- **Value Objects** (`RoutePath`, `HostName`, `CertificateLocation`) enforce domain invariants
- **Repository Pattern** abstracts data access behind ports
- **Use Cases** orchestrate domain logic and trigger proxy reloads
- **Dynamic Config Provider** uses `IServiceProvider` to resolve scoped dependencies from singleton service

## Troubleshooting

### Database Connection Issues

If the API fails to start with database errors:

1. Check PostgreSQL is running: `docker compose ps`
2. Verify connection string is correct
3. Check logs: `docker compose logs api`

### Proxy Not Routing

If routes aren't working:

1. Verify cluster and host exist in database
2. Check route is enabled: `GET /api/routes`
3. Manually trigger reload: `POST /api/proxy/reload`
4. Check YARP logs in API output

## Future Enhancements

- [ ] Blazor WASM UI for management
- [ ] Certificate management API
- [ ] Authentication & Authorization
- [ ] Metrics and monitoring
- [ ] Rate limiting configuration
- [ ] Request transformation rules

## License

This project is licensed under the **Business Source License 1.1 (BUSL-1.1)**.  
It is free for non-commercial use.  
For commercial use, please contact **Deon van Vuuren** for licensing terms.  
After **1 January 2028**, this project will be licensed under the **Apache 2.0 License**.
