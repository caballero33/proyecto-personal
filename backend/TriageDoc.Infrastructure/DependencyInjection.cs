using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TriageDoc.Application.Interfaces;
using TriageDoc.Infrastructure.Data;
using TriageDoc.Infrastructure.Repositories;

namespace TriageDoc.Infrastructure;

/// <summary>
/// Módulo de Inyección de Dependencias para Infraestructura (Clase 5 - DIP).
/// Encapsula el registro del DbContext de EF Core y los Repositorios de datos.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // 1. Configuración de Entity Framework Core con PostgreSQL
        services.AddDbContext<TriageDocDbContext>(options =>
            options.UseNpgsql(connectionString));

        // 2. Registro de Repositorios (Patrón Repositorio y DIP)
        services.AddScoped<IPacienteRepository, PacienteRepository>();
        services.AddScoped<ISintomaRepository, SintomaRepository>();
        services.AddScoped<IProtocoloAtencionRepository, ProtocoloAtencionRepository>();
        services.AddScoped<IEvaluacionTriageRepository, EvaluacionTriageRepository>();

        return services;
    }
}
