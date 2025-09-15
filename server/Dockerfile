FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and sln first (better caching of NuGet restore)
COPY server/src/*.csproj ./server/src/
COPY server/src/server.sln ./server/src/

RUN dotnet restore server/src/server.sln

# Copy everything else
COPY . .

# Build and publish
RUN dotnet publish server/src/server.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "server.dll"]
