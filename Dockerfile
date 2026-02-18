# =========================
# Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only project files first (for better layer caching)
COPY src/Domain/Domain.csproj src/Domain/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/WebApi/WebApi.csproj src/WebApi/

# Restore only WebApi (will restore dependencies transitively)
RUN dotnet restore src/WebApi/WebApi.csproj

# Copy the rest of the source code
COPY src/ src/

# Publish
RUN dotnet publish src/WebApi/WebApi.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# =========================
# Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN groupadd -r appuser && useradd -r -g appuser appuser

COPY --from=build /app/publish .

RUN chown -R appuser:appuser /app
USER appuser

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "WebApi.dll"]