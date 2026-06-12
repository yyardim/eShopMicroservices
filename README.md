# eShopMicroservices

A .NET 10 microservices reference application demonstrating CQRS, clean architecture, and containerised deployment with Docker. Built as a learning project covering real-world patterns used in production microservices systems.

## Architecture Overview

```
src/
├── Services/
│   ├── Catalog/Catalog.API      # Product catalogue management
│   └── Basket/Basket.API        # Shopping basket with Redis caching
├── BuildingBlocks/BuildingBlocks # Shared cross-cutting library
└── docker-compose.yml           # Container orchestration
```

Each service is independently deployable with its own database. Services share nothing at runtime — only the `BuildingBlocks` library is a compile-time dependency.

## Services

### Catalog.API

Manages the product catalogue. Products are stored in PostgreSQL via Marten (document store).

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/products` | List products with pagination (`?pageNumber=1&pageSize=10`) |
| GET | `/products/{id}` | Get product by ID |
| GET | `/products/category/{category}` | Get products by category |
| POST | `/products` | Create a product |
| PUT | `/products/{id}` | Update a product |
| DELETE | `/products/{id}` | Delete a product |
| GET | `/health` | Health check |

**Ports:** HTTP `6000`, HTTPS `6060`

**Product model:**
```json
{
  "id": "guid",
  "name": "string",
  "category": ["string"],
  "description": "string",
  "imageFile": "string",
  "price": 0.00
}
```

### Basket.API

Manages shopping baskets. Uses PostgreSQL (Marten) as the primary store and Redis as a transparent write-through cache via the Decorator pattern.

| Method | Route | Description |
|--------|-------|-------------|
| GET | `/basket/{userName}` | Get basket by user |
| POST | `/basket` | Create or update basket |
| DELETE | `/basket/{userName}` | Delete basket |
| GET | `/health` | Health check |

**Cache strategy:** `CachedBasketRepository` decorates `BasketRepository` — reads check Redis first, writes update both stores. Registered transparently via Scrutor's `Decorate<>`.

## Key Patterns

### CQRS with MediatR

Commands and queries are explicit types routed through MediatR. Every operation follows the same shape:

```
Endpoint  →  ICommand / IQuery  →  MediatR  →  Handler  →  Result
```

Abstractions live in `BuildingBlocks`:
- `ICommand<TResult>` / `IQuery<TResult>` — marker interfaces
- `ICommandHandler<TCommand, TResult>` / `IQueryHandler<TQuery, TResult>` — handler interfaces

### Pipeline Behaviors

Two MediatR behaviors run on every request automatically:

| Behavior | What it does |
|----------|-------------|
| `ValidatorBehavior` | Runs all registered FluentValidation validators; throws `ValidationException` (→ HTTP 400) on failure |
| `LoggingBehavior` | Logs `[START]` with request data and `[END]` with elapsed time; warns if handler takes > 3 seconds |

### Exception Handling

`CustomExceptionHandler` (registered as `IExceptionHandler`) catches all unhandled exceptions and maps them to RFC 7807 `ProblemDetails` responses:

| Exception | HTTP Status |
|-----------|------------|
| `ValidationException` | 400 Bad Request |
| `BadRequestException` | 400 Bad Request |
| `NotFoundException` | 404 Not Found |
| `InternalServerException` | 500 Internal Server Error |
| _(anything else)_ | 500 Internal Server Error |

All responses include a `traceId`. Validation responses also include the full field-level `errors` array.

## Technology Stack

| Concern | Technology |
|---------|-----------|
| Framework | .NET 10, ASP.NET Core Minimal APIs |
| API modules | [Carter](https://github.com/CarterCommunity/Carter) |
| CQRS / Mediator | [MediatR](https://github.com/jbogard/MediatR) |
| Document store | [Marten](https://martendb.io/) on PostgreSQL |
| Distributed cache | Redis via `StackExchange.Redis` |
| Validation | [FluentValidation](https://docs.fluentvalidation.net/) |
| Object mapping | [Mapster](https://github.com/MapsterMapper/Mapster) |
| DI decorators | [Scrutor](https://github.com/khellang/Scrutor) |
| Containerisation | Docker, Docker Compose |
| Health checks | `AspNetCore.HealthChecks.NpgSql` |

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Run with Docker Compose

```bash
cd src
docker compose up --build
```

This starts:
- `catalogdb` — PostgreSQL 17 on port `5432`
- `catalog.api` — Catalog service on port `6000`

The Catalog database is seeded automatically with sample products on first run.

### Run locally (without Docker)

1. Start a PostgreSQL instance and update `ConnectionStrings__Database` in `appsettings.Development.json`.
2. Start a Redis instance for Basket.API.
3. Run each service:

```bash
dotnet run --project src/Services/Catalog/Catalog.API
dotnet run --project src/Services/Basket/Basket.API
```

### Postman collection

A Postman collection is included at `src/Services/Catalog/Catalog.API/Postman/`. Import the collection and the `Catalog API - Local` environment to get pre-configured requests for all endpoints.

## Project Structure — Detailed

```
src/
├── BuildingBlocks/BuildingBlocks/
│   ├── CQRS/                    # ICommand, IQuery, handler interfaces
│   ├── Behaviors/
│   │   ├── LoggingBehavior.cs   # MediatR pipeline: structured request logging
│   │   └── ValidatorBehavior.cs # MediatR pipeline: FluentValidation integration
│   └── Exceptions/
│       ├── BadRequestException.cs
│       ├── NotFoundException.cs
│       ├── InternalServerException.cs
│       └── Handler/
│           └── CustomExceptionHandler.cs
│
├── Services/
│   ├── Catalog/Catalog.API/
│   │   ├── Models/Product.cs
│   │   ├── Products/            # One folder per operation (CQRS slice)
│   │   │   ├── GetProducts/
│   │   │   ├── GetProductById/
│   │   │   ├── GetProductByCategory/
│   │   │   ├── CreateProduct/
│   │   │   ├── UpdateProduct/
│   │   │   └── DeleteProduct/
│   │   ├── Data/CatalogInitialData.cs
│   │   └── Exceptions/ProductNotFoundException.cs
│   │
│   └── Basket/Basket.API/
│       ├── Models/              # ShoppingCart, ShoppingCartItem
│       ├── Basket/              # GetBasket, StoreBasket, DeleteBasket
│       ├── Data/
│       │   ├── IBasketRepository.cs
│       │   ├── BasketRepository.cs       # Marten implementation
│       │   └── CachedBasketRepository.cs # Redis decorator
│       └── Exceptions/BasketNotFoundException.cs
│
└── docker-compose.yml
    docker-compose.override.yml  # Dev ports, env vars, volumes
```

## Roadmap

- Discount service
- Ordering service
- Message bus (RabbitMQ) for async inter-service communication
- API Gateway
