using Microsoft.EntityFrameworkCore;
using TriageDoc.Domain.Entities;

namespace TriageDoc.Infrastructure.Data;

/// <summary>
/// Contexto de Base de Datos para Entity Framework Core y PostgreSQL (Clase 4).
/// Mapea las entidades de Dominio hacia las tablas relacionales.
/// </summary>
public class TriageDocDbContext : DbContext
{
    public TriageDocDbContext(DbContextOptions<TriageDocDbContext> options) : base(options) { }

    public DbSet<Sintoma> Sintomas { get; set; }
    public DbSet<EvaluacionTriage> EvaluacionesTriage { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Semilla inicial básica para pruebas del catálogo de síntomas
        modelBuilder.Entity<Sintoma>().HasData(
            new Sintoma { Id = 1, Nombre = "Dolor Torácico Opresivo", Descripcion = "Dolor en el centro del pecho que irradia a brazo o mandíbula", EsSignoAlarma = true, Categoria = "Cardiovascular" },
            new Sintoma { Id = 2, Nombre = "Dificultad Respiratoria Severa", Descripcion = "Sensación de falta de aire en reposo", EsSignoAlarma = true, Categoria = "Respiratorio" },
            new Sintoma { Id = 3, Nombre = "Fiebre Elevada", Descripcion = "Temperatura axilar superior a 38.5°C", EsSignoAlarma = false, Categoria = "Infeccioso" },
            new Sintoma { Id = 4, Nombre = "Cefalea Leve", Descripcion = "Dolor de cabeza leve a moderado sin alteraciones neurológicas", EsSignoAlarma = false, Categoria = "Neurologico" }
        );
    }
}
