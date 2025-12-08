FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy solution file
COPY server.sln . 

# Copy project files and their directory structure
# Copy the structure needed for the restore step
COPY server/Core/ server/Core/
COPY server/src/ server/src/

# Restore dependencies
RUN dotnet restore server.sln

# Copy all remaining files (source code, etc.)
COPY . .

# Build and publish
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]


