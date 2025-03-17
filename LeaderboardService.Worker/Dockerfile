# Stage 1: Base image (runtime)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Stage 2: Build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy only the csproj file and restore dependencies as a separate layer
COPY ["LeaderboardService.Worker/LeaderboardService.Worker/LeaderboardService.Worker.csproj", "LeaderboardService.Worker/LeaderboardService.Worker/"]
RUN dotnet restore "LeaderboardService.Worker/LeaderboardService.Worker/LeaderboardService.Worker.csproj"

# Copy the rest of the source code into the container
COPY . .

# Set working directory to the project folder and build the project
WORKDIR "/src/LeaderboardService.Worker/LeaderboardService.Worker"
RUN dotnet build "LeaderboardService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish the project for release
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LeaderboardService.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4: Final image (runtime with published files)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LeaderboardService.Worker.dll"]
