# syntax=docker/dockerfile:1

# ---------------------------------------------------------------------------
# Build-Stage
# ---------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Restore separat, damit der Layer bei reinen Code-Aenderungen gecacht bleibt.
COPY Nexora.csproj ./
RUN dotnet restore Nexora.csproj

COPY . ./
RUN dotnet publish Nexora.csproj \
        -c $BUILD_CONFIGURATION \
        -o /app/publish \
        --no-restore \
        /p:UseAppHost=false

# ---------------------------------------------------------------------------
# Runtime-Stage
# ---------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# curl wird fuer den Healthcheck benoetigt (im Basisimage nicht enthalten).
RUN apt-get update \
    && apt-get install -y --no-install-recommends curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish ./

# Production => appsettings.Production.json wird geladen.
ENV ASPNETCORE_ENVIRONMENT=Production

# Der Listenport wird bewusst NICHT hier festgelegt, sondern ueber die
# Konfiguration gesteuert (siehe appsettings.Production.json und docker-compose.yml):
#   "Server": { "Port": 8080 }   oder Umgebungsvariable Server__Port
# EXPOSE ist reine Dokumentation und aendert das Binding nicht.
EXPOSE 8080

# Fallback (:-8080) greift nur, wenn Server__Port nicht gesetzt ist - er muss
# daher dem Default aus appsettings.Production.json entsprechen.
HEALTHCHECK --interval=30s --timeout=5s --start-period=20s --retries=5 \
    CMD curl -fsS "http://localhost:${Server__Port:-8080}/api/auth/status" || exit 1

USER app

ENTRYPOINT ["dotnet", "Nexora.dll"]
