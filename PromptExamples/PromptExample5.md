# 1. Comments

I wanted to add docker infrastructure
Then I wanted to add GitHub Actions based on this docker infrastructions, but didn't saved the prompt
Summary: it went well

# 2. Context

I remember that I specified that I want to build a frontend, backed and DB in docker and specified stack
This time instructions included:
- custom copilot-instructions
- containerization-docker-best-practices.instructions

# 3. Model/Tools

GitHub Copilot (Claude Sonnet 4.6) in VS Code

# 4. Prompt

Overview: The stack is .NET 8 (PostgreSQL, JWT auth) + React 19 (CRA, Vitest). No Docker infrastructure exists. The plan creates three Dockerfiles, a docker-compose.yml, supporting config files, and two targeted code fixes required before images work in containers. All unit tests run inside their respective build stages before the final image is produced. Fully conformant with containerization-docker-best-practices.instructions.md (multi-stage, non-root user, HEALTHCHECK, resource limits, named networks, versioned base images).

Pre-flight Code Fixes
Step 1 — Make CORS origin configurable in Program.cs

Replace the hardcoded http://localhost:3000 with a config-backed value, e.g., read AllowedOrigins from IConfiguration (env var: AllowedOrigins__0). This unblocks Docker Compose injecting the real frontend origin at runtime with no source rebuild.

Step 2 — Conditionalize HTTPS redirection in Program.cs

Wrap app.UseHttpsRedirection() so it only activates when an env var HTTPS_REDIRECT_ENABLED=true is set (defaults off). In Docker, TLS is terminated at the proxy boundary; the container runs plain HTTP on port 5272.

New Files to Create
Step 3 — backend/.dockerignore
Excludes: bin/, obj/, .env*, *.user, IDE folders. Keeps migrations (needed for dotnet ef database update at runtime).

Step 4 — frontend/.dockerignore
Excludes: node_modules/, build/, .env*, coverage output, IDE folders.

Step 5 — frontend/nginx.conf
Minimal nginx config:

Serve CRA build/ as static files on port 80
try_files $uri /index.html for client-side routing
No backend proxy needed (API URL baked into bundle at build time)

Step 6 — backend/entrypoint.sh
Shell script that:

Runs dotnet ef database update (applies pending migrations)
exec dotnet BackendApi.dll (hands off PID 1 properly)

Step 7 — backend/Dockerfile (multi-stage, per best-practices instructions)

Stage	Base Image	What Happens
build	mcr.microsoft.com/dotnet/sdk:8.0-alpine	Restore NuGet (deps layer first), then copy source, dotnet build -c Release
test	from build	Copy ../backend.Tests/, run dotnet test backend.Tests/BackendApi.Tests.csproj — image build aborts on test failure
publish	from build	dotnet publish -c Release -o /app/publish --no-build
final	mcr.microsoft.com/dotnet/aspnet:8.0-alpine	Create non-root appuser, copy publish output + entrypoint.sh, EXPOSE 5272, HEALTHCHECK via wget -qO- http://localhost:5272/health, ENTRYPOINT ["sh", "entrypoint.sh"]

Step 8 — frontend/Dockerfile (multi-stage)

Stage	Base Image	What Happens
deps	node:20-alpine	Copy package.json + package-lock.json, npm ci
test	from deps	Copy source, npm test -- --run (Vitest non-watch) — build aborts on failure
build	from deps	Copy source, npm run build using ARG REACT_APP_API_URL
final	nginx:1.27-alpine	Create non-root nginxuser, copy build/ → nginx html root, copy nginx.conf, EXPOSE 80, HEALTHCHECK via wget -q http://localhost/, CMD ["nginx", "-g", "daemon off;"]

Step 9 — docker-compose.yml

Services:

Service	Image / Build	Published Port	Notes
db	postgres:16-alpine	none (internal only)	Named volume postgres_data; HEALTHCHECK via pg_isready; backend-net only (internal: true)
backend	build backend	5272:5272	depends_on: db: condition: service_healthy; env vars for connection string, JWT settings, CORS, HTTPS_REDIRECT_ENABLED=false; CPU/memory limits
frontend	build frontend	3000:80	build arg REACT_APP_API_URL=http://localhost:5272/api; depends_on: backend; frontend-net only; CPU/memory limits
Networks:

backend-net — db + backend (internal: true)
frontend-net — backend + frontend
Volumes: postgres_data (named)

Step 10 — .env.docker.example (root level)
Template documenting all required env vars (POSTGRES_PASSWORD, JWT_SECRET_KEY, ALLOWED_ORIGINS, etc.) — never committed with real values.

Verification
# 1. Build all images (unit tests run inside build stages)
docker compose build

# 2. Start the stack
docker compose up -d

# 3. Confirm all services reached healthy state
docker compose ps

# 4. Tail logs to confirm DB migration + seeding ran
docker compose logs backend

# 5. Verify backend is up
curl http://localhost:5272/swagger/index.html

# 6. Open frontend
start http://localhost:3000

# 7. Login with seeded admin user to exercise full stack

# 8. Tear down cleanly
docker compose down -v

Node version: node:20-alpine — mandated by vitest instructions (Node.js 20+), supersedes README's "18+".
CRA build-time API URL: ARG REACT_APP_API_URL at docker build time via compose build.args — no frontend code changes needed.
EF Migrations: Run in entrypoint.sh at container startup, guarded by depends_on: db: condition: service_healthy.
HTTPS in container: Disabled via HTTPS_REDIRECT_ENABLED=false env var — TLS is proxy responsibility.
Test stage placement: Tests run before publish/final stages — a failing test aborts the entire image build.
dotnet publish --os linux shortcut: Noted in aspnet instructions but not used — best-practices instructions mandate explicit multi-stage Dockerfiles over .NET's built-in container publishing