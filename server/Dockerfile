FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy the Solution file
COPY server.sln . 

# Copy the server source code
# This now includes the 'Core' submodule content because GitHub Actions fetched it
COPY server/ server/ 

# Re-add the projects to the solution relative to the container structure
RUN dotnet sln add server/src/server.csproj
RUN dotnet sln add server/Core/Core.csproj

# Restore dependencies
RUN dotnet restore server.sln

# Build and Publish
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

# Final Stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]