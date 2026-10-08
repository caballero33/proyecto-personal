using TriageDoc.Application.Features.Evaluaciones.DTOs;
using TriageDoc.Domain.Entities;

namespace TriageDoc.Application.Interfaces;

/// <summary>
/// Contrato del Motor de Inferencia Experto para Triage Médico (Principio DIP - SOLID).
/// Aplica el árbol de decisión basado en el Sistema de Clasificación Manchester.
/// </summary>
public interface ITriageService
{
    string ObtenerEstadoMotor();
    
    /// <summary>
    /// Ejecuta las reglas de inferencia clínica para clasificar el nivel de prioridad de un paciente.
    /// </summary>
    EvaluacionTriage ProcesarInferencia(int pacienteId, int edadPaciente, CreateEvaluacionRequestDto dto);
}
