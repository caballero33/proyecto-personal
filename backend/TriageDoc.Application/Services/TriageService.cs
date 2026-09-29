using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Services;

/// <summary>
/// Implementación del caso de uso de Triage (Principio SRP - La 'S' de SOLID).
/// Su única responsabilidad es procesar las reglas clínicas de decisión y clasificar la urgencia.
/// </summary>
public class TriageService : ITriageService
{
    public string ObtenerEstadoMotor()
    {
        return "Motor Experto TriageDoc v1.0 - Reglas Clínicas de Decisión Activas";
    }

    public EvaluacionTriage EvaluarCasoPreliminar(string nombrePaciente, int edad, bool dolorPecho, bool dificultadRespiratoria, double temperatura)
    {
        var evaluacion = new EvaluacionTriage
        {
            NombrePaciente = nombrePaciente,
            Edad = edad,
            FechaRegistro = DateTime.UtcNow
        };

        // Regla 1: Síntomas vitales graves (Bandera Roja)
        if (dolorPecho && dificultadRespiratoria)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Resucitacion;
            evaluacion.RequiereAtencionInmediata = true;
            evaluacion.JustificacionRegla = "REGLA-01: Sospecha de síndrome coronario o insuficiencia respiratoria severa. Código Rojo.";
            return evaluacion;
        }

        // Regla 2: Compromiso respiratorio aislado o hipertermia severa
        if (dificultadRespiratoria || temperatura >= 39.5)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Emergencia;
            evaluacion.RequiereAtencionInmediata = true;
            evaluacion.JustificacionRegla = "REGLA-02: Dificultad respiratoria detectada o fiebre crítica (>39.5°C). Código Naranja.";
            return evaluacion;
        }

        // Regla 3: Fiebre moderada o paciente de riesgo por edad extrema
        if (temperatura >= 38.0 || edad >= 75 || edad <= 1)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Urgencia;
            evaluacion.RequiereAtencionInmediata = false;
            evaluacion.JustificacionRegla = "REGLA-03: Signos febriles o grupo etario vulnerable. Código Amarillo.";
            return evaluacion;
        }

        // Regla 4: Cuadros leves o estándar
        evaluacion.PrioridadAsignada = NivelPrioridad.Prioritario;
        evaluacion.RequiereAtencionInmediata = false;
        evaluacion.JustificacionRegla = "REGLA-04: Signos vitales sin compromiso inmediato. Código Verde.";

        return evaluacion;
    }
}
