# ==============================================================================
# TEMA 4: CONTENERIZACIÓN DEL BACKEND (.NET 8 CLEAN ARCHITECTURE)
# Estrategia: Multi-Stage Build (SDK para Compilación -> ASP.NET Runtime Ligero)
# ==============================================================================

# ------------------------------------------------------------------------------
# ETAPA 1: Compilación y Publicación (SDK de .NET 8)
# Contiene compiladores, CLI de NuGet y herramientas de construcción.
# ------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src

# 1. Copiar archivos de solución y definición de proyectos (.csproj) por capa
# Esto optimiza la caché de capas de Docker si solo cambia el código C#.
COPY ["TriageDoc.sln", "./"]
COPY ["TriageDoc.Domain/TriageDoc.Domain.csproj", "TriageDoc.Domain/"]
COPY ["TriageDoc.Application/TriageDoc.Application.csproj", "TriageDoc.Application/"]
COPY ["TriageDoc.Infrastructure/TriageDoc.Infrastructure.csproj", "TriageDoc.Infrastructure/"]
COPY ["TriageDoc.API/TriageDoc.API.csproj", "TriageDoc.API/"]

# 2. Restaurar dependencias NuGet de toda la arquitectura limpia
RUN dotnet restore "TriageDoc.API/TriageDoc.API.csproj"

# 3. Copiar el código fuente completo de las 4 capas y compilar en modo Release
COPY . .
WORKDIR "/src/TriageDoc.API"
RUN dotnet publish "TriageDoc.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ------------------------------------------------------------------------------
# ETAPA 2: Imagen Final de Producción (ASP.NET Core Runtime Ultraligero)
# Excluye el SDK y código fuente; solo incluye los binarios compilados.
# ------------------------------------------------------------------------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /app

# Exponer puerto HTTP interno del contenedor
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development

# Copiar únicamente los artefactos compilados desde la Etapa 1 (build)
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TriageDoc.API.dll"]
