using TriageDoc.Application.Features.Evaluaciones.DTOs;
using TriageDoc.Application.Interfaces;
using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Services;

/// <summary>
/// Motor de Inferencia Experto de Triage Médico (Principio SRP - La 'S' de SOLID).
/// Su única responsabilidad es procesar la base de conocimiento y reglas clínicas de decisión (Manchester Triage System).
/// </summary>
public class TriageService : ITriageService
{
    public string ObtenerEstadoMotor()
    {
        return "Motor Experto TriageDoc v2.0 - Sistema Manchester 5 Niveles Activo y Calibrado";
    }

    public EvaluacionTriage ProcesarInferencia(int pacienteId, int edadPaciente, CreateEvaluacionRequestDto dto)
    {
        var evaluacion = new EvaluacionTriage
        {
            PacienteId = pacienteId,
            FechaRegistro = DateTime.UtcNow,
            Temperatura = dto.Temperatura,
            FrecuenciaCardiaca = dto.FrecuenciaCardiaca,
            PresionArterial = dto.PresionArterial,
            SaturacionOxigeno = dto.SaturacionOxigeno,
            DolorToracico = dto.DolorToracico,
            DificultadRespiratoria = dto.DificultadRespiratoria,
            SintomasPrincipales = dto.SintomasPrincipales,
            ObservacionesClinicas = dto.ObservacionesClinicas
        };

        // REGLA 1: Código Rojo (Resucitación / Nivel 1)
        // Compromiso cardiovascular o respiratorio severo inminente
        if ((dto.DolorToracico && dto.DificultadRespiratoria) || dto.SaturacionOxigeno < 90 || dto.FrecuenciaCardiaca > 150 || dto.FrecuenciaCardiaca < 40)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Resucitacion;
            evaluacion.ProtocoloAtencionId = 1;
            evaluacion.RequiereAtencionInmediata = true;
            evaluacion.JustificacionRegla = "REGLA-01 [NIVEL 1]: Paciente con signos de paro inminente, hipoxemia severa (SpO2 < 90%) o compromiso hemodinámico extremo. Atención inmediata en Shock Trauma.";
            return evaluacion;
        }

        // REGLA 2: Código Naranja (Emergencia / Nivel 2)
        // Compromiso respiratorio o hemodinámico moderado-alto, o hipertermia crítica
        if (dto.DificultadRespiratoria || dto.DolorToracico || dto.Temperatura >= 39.5 || (dto.SaturacionOxigeno >= 90 && dto.SaturacionOxigeno <= 93) || dto.FrecuenciaCardiaca > 125)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Emergencia;
            evaluacion.ProtocoloAtencionId = 2;
            evaluacion.RequiereAtencionInmediata = true;
            evaluacion.JustificacionRegla = "REGLA-02 [NIVEL 2]: Dificultad respiratoria detectada, dolor torácico aislado, fiebre muy alta (>39.5°C) o taquicardia severa. Atención en menos de 10 min.";
            return evaluacion;
        }

        // REGLA 3: Código Amarillo (Urgencia / Nivel 3)
        // Paciente con signos febriles, taquicardia moderada o grupo etario vulnerable
        if (dto.Temperatura >= 38.0 || dto.FrecuenciaCardiaca > 100 || edadPaciente >= 75 || edadPaciente <= 1)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Urgencia;
            evaluacion.ProtocoloAtencionId = 3;
            evaluacion.RequiereAtencionInmediata = false;
            evaluacion.JustificacionRegla = $"REGLA-03 [NIVEL 3]: Síndrome febril (Temp: {dto.Temperatura}°C) o paciente en grupo de riesgo etario ({edadPaciente} años). Atención en menos de 60 min.";
            return evaluacion;
        }

        // REGLA 4: Código Verde (Prioritario / Poco Urgente / Nivel 4)
        // Síntomas moderados o leves con signos vitales dentro de rangos aceptables
        if (!string.IsNullOrWhiteSpace(dto.SintomasPrincipales) && dto.SintomasPrincipales.ToLower().Contains("leve") || dto.Temperatura > 37.5)
        {
            evaluacion.PrioridadAsignada = NivelPrioridad.Prioritario;
            evaluacion.ProtocoloAtencionId = 4;
            evaluacion.RequiereAtencionInmediata = false;
            evaluacion.JustificacionRegla = "REGLA-04 [NIVEL 4]: Signos vitales normales con síntomas agudos leves a moderados sin bandera roja. Atención en menos de 120 min.";
            return evaluacion;
        }

        // REGLA 5: Código Azul (No Urgente / Nivel 5)
        // Sin compromiso agudo de salud
        evaluacion.PrioridadAsignada = NivelPrioridad.NoUrgente;
        evaluacion.ProtocoloAtencionId = 5;
        evaluacion.RequiereAtencionInmediata = false;
        evaluacion.JustificacionRegla = "REGLA-05 [NIVEL 5]: Parámetros fisiológicos estables sin banderas rojas identificadas. Atención en menos de 240 min.";

        return evaluacion;
    }
}
