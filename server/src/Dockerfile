# Stage 1: Build the application for release (Production)
# This stage compiles and publishes the app for a lean production environment.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY ["server.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet publish "server.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Create the final production image
# This stage takes only the published artifacts for a small and secure production image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# Assumes your project's output DLL is named server.dll
ENTRYPOINT ["dotnet", "server.dll"]

# Stage 3: Development stage
# This stage uses the full SDK and is intended to be run with `dotnet watch`.
# It will be targeted by the docker-compose.yml file for development.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS dev
WORKDIR /app
EXPOSE 80

# Copy the project file and restore dependencies first.
# This leverages Docker's layer caching. When only your .cs files change,
# this layer won't need to be rebuilt, speeding up subsequent builds.
COPY ["server.csproj", "./"]
RUN dotnet restore

# Copy the rest of the source code. This will be overlaid by the volume mount
# in docker-compose, but it's good practice to have it in the image for completeness.
COPY . .

# The command to start the app with hot-reload will be specified in the docker-compose.yml
# This makes the image more flexible.