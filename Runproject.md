# Running with Docker (Recommended)

## 1. Build the images

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

# Manual run
## 1. DB Setup
1. Create a database:
   ```sql
   CREATE DATABASE backendapi_dev_db;
   ```
2. Update the connection string in `backend/appsettings.Development.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=backendapi_dev_db;Username=your_username;Password=your_password"
     }
   }
   ```

## 2. Backend Setup

```bash
cd backend

# Apply database migrations
dotnet ef database update

# Run the backend
dotnet run
```

The API will be available at:
- HTTPS: https://localhost:7181
- HTTP: http://localhost:5272
- Swagger UI: https://localhost:7181/swagger

## 3. Frontend Setup

```bash
cd frontend

# Install dependencies
npm install

# Start the React development server
npm start
```

## 4. Running Tests
**Backend Unit Tests:**

### Build projects
```bash
dotnet build backend/BackendApi.csproj -c Release --no-restore
dotnet build backend.Tests/BackendApi.Tests.csproj -c Release --no-restore
```

### Run tests
```bash
dotnet test backend.Tests/BackendApi.Tests.csproj
```

**Frontend Unit Tests (Vitest):**
```bash
cd frontend

# Run tests in watch mode
npm test

# Run tests with UI
npm run test:ui

# Run tests with coverage report
npm run test:coverage
```