# Base runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# SDK build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/MachineInsight.API/MachineInsight.API.csproj", "MachineInsight.API/"]
COPY ["src/MachineInsight.Application/MachineInsight.Application.csproj", "MachineInsight.Application/"]
COPY ["src/MachineInsight.Infrastructure/MachineInsight.Infrastructure.csproj", "MachineInsight.Infrastructure/"]
COPY ["src/MachineInsight.Domain/MachineInsight.Domain.csproj", "MachineInsight.Domain/"]

WORKDIR /src/MachineInsight.API
RUN dotnet restore

COPY src/ ./
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build /p:UseAppHost=false

# Publish
FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final API runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MachineInsight.API.dll"]