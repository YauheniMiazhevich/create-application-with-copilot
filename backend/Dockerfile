# =============================================================================
# Stage 1 – build
# Restore NuGet packages (separate layer for cache efficiency) then compile.
# Build context: workspace root (needed to access both backend/ and backend.Tests/)
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# Copy project files first so NuGet restore layer is cached independently of source changes
COPY backend/BackendApi.csproj backend/
COPY backend.Tests/BackendApi.Tests.csproj backend.Tests/
RUN dotnet restore backend/BackendApi.csproj && \
    dotnet restore backend.Tests/BackendApi.Tests.csproj

# Copy source
COPY backend/ backend/
COPY backend.Tests/ backend.Tests/

# Compile (Release) – both main project and test project
RUN dotnet build backend/BackendApi.csproj -c Release --no-restore && \
    dotnet build backend.Tests/BackendApi.Tests.csproj -c Release --no-restore

# =============================================================================
# Stage 2 – test
# Run all unit tests. A failing test aborts the entire image build.
# =============================================================================
FROM build AS test
RUN dotnet test backend.Tests/BackendApi.Tests.csproj \
        -c Release \
        --no-build \
        --logger "console;verbosity=normal"

# =============================================================================
# Stage 3 – publish
# Inherits from test so tests MUST pass before publish runs.
# =============================================================================
FROM test AS publish
RUN dotnet publish backend/BackendApi.csproj \
        -c Release \
        -o /app/publish \
        --no-build

# =============================================================================
# Stage 4 – final
# Minimal ASP.NET runtime image. Non-root user. Health check included.
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# Create a dedicated non-root user
RUN addgroup -S appgroup && adduser -S appuser -G appgroup

# Copy published output from publish stage
COPY --from=publish --chown=appuser:appgroup /app/publish .

# Switch to non-root user
USER appuser

# Document the HTTP port (HTTPS handled externally by a reverse proxy)
EXPOSE 5272

# Health check – use 127.0.0.1 explicitly; localhost may resolve to ::1 (IPv6).
HEALTHCHECK --interval=30s --timeout=5s --start-period=30s --retries=3 \
    CMD wget -qO- http://127.0.0.1:5272/health || exit 1

# Exec form for proper PID 1 / signal handling
# Migrations are applied automatically at startup via Database.MigrateAsync() in Program.cs
CMD ["dotnet", "BackendApi.dll"]
