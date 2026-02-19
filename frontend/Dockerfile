# =============================================================================
# Stage 1 – deps
# Install all npm dependencies (including devDependencies for build + tests).
# Separating this layer means npm ci only re-runs when package-lock.json changes.
# =============================================================================
FROM node:20-alpine AS deps
WORKDIR /app

COPY package.json package-lock.json ./
RUN npm ci && npm cache clean --force

# =============================================================================
# Stage 2 – test
# Run Vitest in single-pass (--run) mode. A failing test aborts the image build.
# =============================================================================
FROM deps AS test
COPY . .
RUN npx vitest --run

# =============================================================================
# Stage 3 – build
# Bake the API URL into the CRA bundle at build time via ARG.
# Inherits from test so tests MUST pass before the production bundle is built.
# =============================================================================
FROM test AS build
ARG REACT_APP_API_URL=http://localhost:5272/api
ENV REACT_APP_API_URL=$REACT_APP_API_URL

# Disable CRA's ESLint-as-warnings-are-errors in CI (avoids false build failures)
ENV CI=false

RUN npm run build

# =============================================================================
# Stage 4 – final
# Minimal nginx image serving the static CRA bundle. Non-root user.
# =============================================================================
FROM nginx:1.27-alpine AS final

# Redirect the nginx pid file to /tmp so the non-root user can write it.
# nginx uses 'pid /var/run/nginx.pid' which symlinks to /run/nginx.pid – patch both forms.
RUN sed -i 's|pid\s*/[a-z/]*nginx\.pid\s*;|pid /tmp/nginx.pid;|' /etc/nginx/nginx.conf && \
    # Also redirect the worker temp-path directories to /tmp (non-root writable)
    addgroup -S nginxgroup && adduser -S nginxuser -G nginxgroup && \
    chown -R nginxuser:nginxgroup \
        /usr/share/nginx/html \
        /var/cache/nginx \
        /var/log/nginx \
        /etc/nginx/conf.d

# Copy custom nginx config and the built React app
COPY --chown=nginxuser:nginxgroup nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build --chown=nginxuser:nginxgroup /app/build /usr/share/nginx/html

USER nginxuser

EXPOSE 8080

# Health check – use 127.0.0.1 explicitly; localhost may resolve to ::1 (IPv6)
# but nginx listens on 0.0.0.0 (IPv4) only.
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
    CMD wget -qO- http://127.0.0.1:8080/healthz || exit 1

CMD ["nginx", "-g", "daemon off;"]
