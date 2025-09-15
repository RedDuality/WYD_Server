# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy only project files for better restore caching
COPY server/src/*.csproj ./server/src/
COPY server/Core/*.csproj ./server/Core/
COPY server/server.sln ./server/

# Restore dependencies
RUN dotnet restore server/server.sln

# Copy the rest of the source
COPY . .

# Build first to leverage caching
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]
