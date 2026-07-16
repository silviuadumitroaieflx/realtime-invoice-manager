# FacturiApp — Real-Time Invoice Management

Invoice management application (clients, products, invoices, payments) built twice with the same feature set, to compare two architectures:

| | **FacturiApp/** | **FacturiAppMs/** |
|---|---|---|
| Architecture | Monolith | Microservices (5 services) |
| Backend | ASP.NET Core (.NET 10) | 5 × ASP.NET Core (.NET 10) |
| Database | 1 × SQL Server | 1 database per service |
| Real-time | SignalR (WebSocket) | SignalR (WebSocket) |
| Auth | JWT Bearer | Dedicated AuthService + JWT |

The frontend is the same in both versions: **Vue 3** (CDN, no build step) + the SignalR JavaScript client.

## Features

- **Authentication** — register, login, password reset; JWT Bearer tokens
- **Clients** — CRUD, with **real-time sync**: when a client is added/edited/deleted, all connected browsers update instantly via a SignalR hub (`/clientHub`)
- **Products** — CRUD with stock and price
- **Invoices** — created from clients + product lines (many-to-many), automatic total
- **Payments** — recorded per invoice
- **Swagger UI** — interactive API documentation on every service

## Tech stack

ASP.NET Core (.NET 10) · Entity Framework Core · SQL Server · SignalR · JWT · Vue 3 · Swagger

## Architecture

### Monolith (`FacturiApp/`)

```
frontend (Vue 3)  ──HTTP/WebSocket──►  backend :5279
                                        ├── Controllers (Auth, Client, Produs, Factura, Plata)
                                        ├── Hubs/ClientHub (SignalR)
                                        └── SQL Server: Facturi
```

### Microservices (`FacturiAppMs/`)

Each service owns its controllers, its `DbContext` and **its own database** (database-per-service pattern):

| Service | Port | Database |
|---|---|---|
| ClientService (+ SignalR hub) | 5101 | FacturiClienti |
| ProdusService | 5102 | FacturiProduse |
| FacturaService | 5103 | FacturiFacturi |
| PlatiService | 5104 | FacturiPlati |
| AuthService | 5105 | FacturiAuth |

The frontend calls each service directly and keeps the client list in sync through ClientService's SignalR hub.

## Running locally

**Prerequisites:** .NET 10 SDK, SQL Server Express (`localhost\SQLEXPRESS`, Windows Authentication).

### Monolith

```powershell
cd FacturiApp/backend
dotnet ef database update   # creates the Facturi database
dotnet run                  # starts on http://localhost:5279
```

Then open `FacturiApp/frontend/index.html` in a browser.

### Microservices

```powershell
cd FacturiAppMs/backend
# create each service's database (run once per service):
#   cd <ServiceName>; dotnet ef database update; cd ..
./start-all.ps1             # starts all 5 services (ports 5101-5105)
```

Then open `FacturiAppMs/frontend/index.html` in a browser.

> **Note:** the JWT signing keys in `appsettings.json` are development-only placeholders. For any real deployment, move them to environment variables or user secrets.
