# Stage 1: Build the application for release (Production)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the entire project context into the container
# This preserves the directory structure (e.g., src/, Core/, etc.)
COPY . .

# Restore all project dependencies by specifying the solution file
RUN dotnet restore src/server.sln

# Publish the application
RUN dotnet publish "src/server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final production image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]