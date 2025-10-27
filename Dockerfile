# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY TaskPulse.sln ./
COPY TaskPulse.Api/TaskPulse.Api.csproj TaskPulse.Api/
COPY TaskPulse.Core/TaskPulse.Core.csproj TaskPulse.Core/
COPY TaskPulse.Infrastructure/TaskPulse.Infrastructure.csproj TaskPulse.Infrastructure/

RUN dotnet restore

COPY . .

RUN dotnet publish TaskPulse.Api -c Release -o /app/publish

# Stage 2: runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "TaskPulse.Api.dll"]
