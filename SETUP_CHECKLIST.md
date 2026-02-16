# Development Setup Checklist

## Prerequisites Installation Status

- [ ] .NET 8 SDK installed
- [ ] Node.js (v18+) installed
- [ ] PostgreSQL (v12+) installed and running
- [ ] Git installed

## Setup Steps

### Database Setup
- [ ] PostgreSQL server is running
- [ ] Database `backendapi_dev_db` created
- [ ] Connection string updated in `backend/appsettings.Development.json`

### Backend Setup
- [ ] Navigate to `backend/` directory
- [ ] Run `dotnet restore`
- [ ] Run `dotnet ef database update` (applies migrations and seeds admin user)
- [ ] Verify backend runs with `dotnet run`
- [ ] Check Swagger UI at https://localhost:7181/swagger
- [ ] Verify auth endpoints exist: `/api/auth/register`, `/api/auth/login`, `/api/auth/me`

### Frontend Setup
- [ ] Navigate to `frontend/` directory
- [ ] Run `npm install`
- [ ] Verify `.env` file exists with correct API URL
- [ ] Run `npm start`
- [ ] Check React app at http://localhost:3000

## Authentication Verification

- [ ] App redirects unauthenticated users to login page
- [ ] Can register new user with email/password
- [ ] Registration assigns "User" role by default
- [ ] Can login with registered user
- [ ] Can login with admin account (admin@backendapi.com / Admin123!)
- [ ] Token is stored in localStorage
- [ ] After login, user is redirected to /products
- [ ] Can view products list
- [ ] Can create new products
- [ ] Can delete products
- [ ] Logout button works and redirects to login
- [ ] After logout, cannot access /products without logging back in

## Manual Testing

**Terminal 1 (Backend):**
```bash
cd backend
dotnet ef database update  # Only needed first time
dotnet run
```

**Terminal 2 (Frontend):**
```bash
cd frontend
npm start
```

## Verification Checklist

- [ ] Backend API responds at https://localhost:7181
- [ ] Swagger documentation accessible at https://localhost:7181/swagger
- [ ] Frontend loads at http://localhost:3000
- [ ] Redirected to /login when not authenticated
- [ ] Login page displays with toggle to register
- [ ] Can register new user
- [ ] Can login with admin credentials shown on login page
- [ ] After login, see header with user email and logout button
- [ ] Products page displays product list
- [ ] Can create new products
- [ ] Can delete products
- [ ] Browser console shows no errors
- [ ] No CORS errors in network tab
- [ ] Attempting to access /products when logged out redirects to /login

## Common Issues

### PostgreSQL Connection Error
- Ensure PostgreSQL is running
- Verify credentials in connection string
- Check if database exists

### Migration Errors
- Delete `Migrations` folder and recreate: `dotnet ef migrations add AddIdentity`
- Apply migrations: `dotnet ef database update`

### "Password authentication failed"
- Update connection string with correct PostgreSQL username/password

### CORS Error in Browser
- Ensure backend is running before frontend
- Verify CORS policy includes http://localhost:3000

### SSL Certificate Warning
Run: `dotnet dev-certs https --trust`

### Port Already in Use
- Backend: Change ports in `backend/Properties/launchSettings.json`
- Frontend: Set PORT environment variable (e.g., `PORT=3001 npm start`)

### 401 Unauthorized Errors
- Clear localStorage in browser DevTools
- Try logging out and back in
- Check token expiration (default: 4 hours)

### Build Errors
Run `dotnet build` in backend folder to see specific errors
