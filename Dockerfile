# Etapa base com runtime do ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Etapa de build com SDK do .NET
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia o .csproj e restaura os pacotes
COPY ["src/MachineInsight.API/MachineInsight.API.csproj", "MachineInsight.API/"]
WORKDIR /src/MachineInsight.API
RUN dotnet restore

COPY src/MachineInsight.API/. ./ 
RUN dotnet build -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MachineInsight.API.dll"]
