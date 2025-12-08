FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY server.sln . 
COPY server/ server/ 

# --- Fix Path Separator Issue ---
# 2. Remove the existing projects from the solution
RUN dotnet sln remove server/src/server.csproj server/Core/Core.csproj

# 3. Add projects back using Linux-style forward slashes
RUN dotnet sln add server/src/server.csproj
RUN dotnet sln add server/Core/Core.csproj
# ---------------------------------

# 4. Restore dependencies (now the paths are correct)
RUN dotnet restore server.sln

# 5. Copy everything else (e.g., appsettings.json, code files)
COPY . .

# Build and publish
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]


