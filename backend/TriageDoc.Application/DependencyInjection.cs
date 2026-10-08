using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TriageDoc.Application.Interfaces;
using TriageDoc.Application.Services;

namespace TriageDoc.Application;

/// <summary>
/// Módulo de Inyección de Dependencias para la Capa de Aplicación (Clase 8).
/// Registra automáticamente todos los validadores de FluentValidation y los servicios de casos de uso.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // 1. Escaneo y registro automático de validadores de FluentValidation en este ensamblado
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // 2. Registro del Motor Experto de Triage
        services.AddScoped<ITriageService, TriageService>();

        return services;
    }
}
