FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY server.sln . 

# 2. Copy the entire 'server' directory which contains all project files.
# This results in: /app/server/Core/Core.csproj and /app/server/src/server.csproj
COPY server/ server/ 

# 3. Restore dependencies
# The solution file is at /app/server.sln and it finds the projects at
# /app/server/Core/Core.csproj, matching what is registered in the SLN file.
RUN dotnet restore server.sln

# 4. Copy everything else (for code/config files not needed for restore)
COPY . .

# Build and publish
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]


