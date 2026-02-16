# .NET MVC + React Application

A full-stack web application built with ASP.NET Core Web API (.NET 8) backend and React frontend, featuring **JWT authentication**, user management, and product/category management with PostgreSQL database.

## Project Structure

```
├── backend/                 # ASP.NET Core Web API
│   ├── Controllers/        # API controllers (Auth, Products, Categories)
│   ├── Data/              # DbContext and database seeding
│   ├── Models/            # Entity models and DTOs
│   │   └── DTOs/          # Data Transfer Objects
│   └── Migrations/        # EF Core migrations
└── frontend/               # React application
    ├── public/            # Static files
    └── src/
        ├── components/    # React components
        │   └── Auth/      # Authentication components
        ├── contexts/      # React contexts (Auth)
        └── services/      # API service layer
```

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (v18 or higher)
- [PostgreSQL](https://www.postgresql.org/download/) (v12 or higher)
- [Git](https://git-scm.com/)

## Setup Instructions

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
Default Login Credentials

After running migrations, the following admin account is automatically created:

**Admin Account:**
- Email: `admin@backendapi.com`
- Password: `Admin123!`

You can also register new users through the application interface. New users are assigned the "User" role by default.

## API Endpoints

### Authentication (Public)
- `POST /api/auth/register` - Register new user account
- `POST /api/auth/login` - Login and receive JWT token
- `GET /api/auth/me` - Get current user info (requires authentication)

### Products (Protected - Requires Authentication)
- `G**JWT Authentication** with ASP.NET Core Identity
- ✅ **User Registration & Login** with role-based access (Admin/User)
- ✅ **Protected API endpoints** requiring authentication
- ✅ **Automatic token management** with axios interceptors
- ✅ RESTful API with ASP.NET Core Web API
- ✅ Entity Framework Core with PostgreSQL
- ✅ Code-First migrations with automatic seeding
- ✅ CORS configuration for cross-origin requests
- ✅ React frontend with hooks and Context API
- ✅ React Router for navigation and protected route` - Delete product

### Categories (Protected - Requires Authentication)
- `GET /api/categories` - Get all categories
- `GET /api/categories/{id}` - Get category by ID
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

**Note:** All Product and Category endpoints require a valid JWT token in the Authorization header.
- `POST /api/categories` - Create new category
- `PUT /api/categories/{id}` - Update category
- `DELETE /api/categories/{id}` - Delete category

## Features

- ✅ RESTful API with ASP.NET Core Web API
- ✅ Entity Framework Core with PostgreSQL
- ✅ Code-First migrations
- ✅ CORS configuration for cross-origin requests
- ✅ React frontend with hooks
- ✅ Axios for HTTP requests
- ✅ CRUD operations for Products and Categories
- ✅ Responsive UI design
- ✅ Error handling and loading states
- ✅ Swagger/OpenAPI documentation

## Development

### Running Migrations

```bash
cd backend

# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations
- `JwtSettings:SecretKey` - Secret key for JWT token generation (min 32 characters)
- `JwtSettings:Issuer` - Token issuer (e.g., "BackendApi")
- `JwtSettings:Audience` - Token audience (e.g., "BackendApiClients")  
- `JwtSettings:ExpirationHours` - Token expiration time in hours (default: 4)
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```
ASP.NET Core Identity
- JWT Bearer Authentication
- Entity Framework Core 8.0
- Npgsql (PostgreSQL provider)
- Swagger/OpenAPI

### Frontend
- React 19
- React Router DOM
- Axios
- JWT Decodepublish -c Release -o ./publish
```

**Frontend:**
```bash
cd fAuthentication Issues
- Verify JWT settings are configured in appsettings.json
- Check that migrations have been applied (admin user should be seeded)
- Clear browser localStorage and try logging in again
- Check browser console for token-related errors

### CORS Issues
- Ensure the backend is running before starting the frontend
- Verify CORS policy in `Program.cs` includes the React app origin

### Database Connection Issues
- Verify PostgreSQL is running
- Check connection string credentials
- Ensure the database exists

### SSL Certificate Issues
- Trust the development certificate:
  ```bash
  dotnet dev-certs https --trust
  ```

### "401 Unauthorized" Errors
- Ensure you're logged in (token is stored in localStorage)
- Check if token has expired (default: 4 hours)
- Try logging out and logging back in
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

## Troubleshooting

### CORS Issues
- Ensure the backend is running before starting the frontend
- Verify CORS policy in `Program.cs` includes the React app origin

### Database Connection Issues
- Verify PostgreSQL is running
- Check connection string credentials
- Ensure the database exists

### SSL Certificate Issues
- Trust the development certificate:
  ```bash
  dotnet dev-certs https --trust
  ```

## License

MIT