FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY server.sln . 
COPY server/ server/ 

# --- DEBUG STEP ---
# This will list all files in 'server/' so you can see the exact casing (Core vs core)
RUN ls -R server/ 
# ------------------

RUN dotnet sln remove server/src/server.csproj server/Core/Core.csproj
RUN dotnet sln add server/src/server.csproj

# Update this line based on the output of the 'ls' command above
RUN dotnet sln add server/Core/Core.csproj
COPY . .

# Build and publish
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]


