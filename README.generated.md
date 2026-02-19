# .NET Web API + React Application

A full-stack real estate management application built with ASP.NET Core Web API (.NET 8) backend with PostgreSQL database and React frontend.

## Project Structure

```
├── backend/                        # ASP.NET Core Web API
│   ├── Controllers/               # API controllers (Auth, Companies, Owners, Properties, PropertyTypes)
│   ├── Data/                      # DbContext and database seeding
│   ├── Interfaces/                # Repository and service interfaces
│   ├── Migrations/                # EF Core migrations
│   ├── Models/                    # Entity models
│   │   └── DTOs/                  # Data Transfer Objects
│   ├── Properties/                # Launch settings
│   ├── Repositories/              # Data access layer
│   ├── Services/                  # Business logic layer
│   └── Validators/                # FluentValidation validators
├── backend.Tests/                  # NUnit backend test project
│   ├── Controllers/               # Controller tests
│   ├── Helpers/                   # Test utilities
│   ├── Repositories/              # Repository tests
│   ├── Services/                  # Service tests
│   └── Validators/                # Validator tests
└── frontend/                       # React application
    ├── public/                    # Static files
    └── src/
        ├── components/            # React components
        │   ├── Auth/              # Authentication components
        │   ├── common/            # Shared UI components (Header, Modal, ErrorBoundary, etc.)
        │   ├── Owners/            # Owner form components
        │   ├── Properties/        # Property card, form and detail components
        │   └── Sidebar/           # Navigation sidebar
        ├── contexts/              # React contexts (Auth)
        ├── hooks/                 # Custom React hooks (useProperties, useOwners, usePropertyTypes)
        ├── services/              # API service layer
        └── utils/                 # Utility functions
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [PostgreSQL](https://www.postgresql.org/download/) (v12 or higher)

---

## Running with Docker (Recommended)

Docker is the fastest way to get the full stack running. It starts PostgreSQL, the .NET backend, and the React frontend — all pre-configured and wired together. Unit tests run automatically inside the image builds; a failing test will abort the build before any container is started.

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (includes Docker Compose)

### 1. Build the images

Unit tests (91 NUnit backend tests + 182 Vitest frontend tests) run **inside** the build. If any test fails, the build stops before a broken image is produced.

```bash
docker compose build
```

To rebuild from scratch (ignoring the layer cache):

```bash
docker compose build --no-cache
```

### 2. Start the stack

```bash
docker compose up -d
```

This starts three containers:

| Service | URL | Notes |
|---|---|---|
| `frontend` | <http://localhost:3000> | React app (nginx) |
| `backend` | <http://localhost:5272> | ASP.NET Core API |
| `db` | internal only | PostgreSQL 16 (not exposed to host) |

Migrations are applied and seed data is loaded automatically on first start.

### 3. Verify everything is healthy

```bash
docker compose ps
```

All three services should show `(healthy)` within ~30 seconds. You can also probe the endpoints directly:

```bash
# Backend health
curl http://localhost:5272/health

# Swagger UI
open http://localhost:5272/swagger

# Frontend
open http://localhost:3000
```

### 4. View logs

```bash
# All services
docker compose logs -f

# Single service
docker compose logs -f backend
docker compose logs -f frontend
```

### 5. Stop the stack

```bash
# Stop containers (data is preserved in the named volume)
docker compose down

# Stop and remove all data (full reset)
docker compose down -v
```

### Rebuilding after code changes

After changing backend or frontend source, rebuild only the affected service:

```bash
docker compose build backend
docker compose up -d backend

# or for frontend
docker compose build frontend
docker compose up -d frontend
```

### Passing a custom API URL to the frontend

The React bundle bakes the API URL in at build time. Override it via the build arg:

```bash
REACT_APP_API_URL=https://api.example.com docker compose build frontend
```

Or set `REACT_APP_API_URL` in your `.env` file before running `docker compose build`.


### 1. Clone the Repository

```bash
git clone <repository-url>
cd create-application-with-copilot
```

### 2. Database Setup

1. Install and start PostgreSQL
2. Create a database:
   ```sql
   CREATE DATABASE backendapi_dev_db;
   ```
3. Update the connection string in `backend/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=backendapi_dev_db;Username=your_username;Password=your_password"
     }
   }
   ```

### 3. Backend Setup

```bash
cd backend

# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update

# Run the backend
dotnet run
```

The API will be available at:
- HTTPS: https://localhost:7181
- HTTP: http://localhost:5272
- Swagger UI: https://localhost:7181/swagger

### 4. Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Create .env file (if not exists)
cp .env.example .env

# Start the React development server
npm start
```

### 5. Running Tests

**Frontend Unit Tests (Vitest):**

# Run all tests (single run)
npm test -- --run

# Run tests in watch mode
npm test

# Run tests with UI
npm run test:ui

# Run tests with coverage report
npm run test:coverage
```

## Features

- ✅ RESTful API with ASP.NET Core Web API
- ✅ Entity Framework Core with PostgreSQL
- ✅ Code-First migrations
- ✅ CORS configuration for cross-origin requests
- ✅ React frontend with hooks
- ✅ Axios for HTTP requests
- ✅ Vitest for frontend unit testing
- ✅ Error handling and loading states

## Development

### Running Migrations

```bash
cd backend

# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Frontend
- React 19
- React Router DOM
- Axios
- JWT Decodepublish -c Release -o ./publish
```

## Technologies Used

### Backend
- ASP.NET Core 8.0
- Entity Framework Core 8.0
- Npgsql (PostgreSQL provider)
- Swagger/OpenAPI

### Frontend
- React 18
- Axios
- Create React App
- CSS3

## License

MIT