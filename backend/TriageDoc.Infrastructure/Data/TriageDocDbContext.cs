using Microsoft.EntityFrameworkCore;
using TriageDoc.Domain.Entities;

namespace TriageDoc.Infrastructure.Data;

/// <summary>
/// Contexto de Base de Datos para Entity Framework Core y PostgreSQL (Clase 4 y 6).
/// Mapea las 4 entidades de Dominio hacia las tablas relacionales normalizadas en 3FN.
/// </summary>
public class TriageDocDbContext : DbContext
{
    public TriageDocDbContext(DbContextOptions<TriageDocDbContext> options) : base(options) { }

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Sintoma> Sintomas => Set<Sintoma>();
    public DbSet<ProtocoloAtencion> ProtocolosAtencion => Set<ProtocoloAtencion>();
    public DbSet<EvaluacionTriage> EvaluacionesTriage => Set<EvaluacionTriage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Paciente
        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("pacientes");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Id).HasColumnName("id");
            entity.HasIndex(p => p.Dni).IsUnique();
            entity.Property(p => p.Dni).HasColumnName("dni").IsRequired().HasMaxLength(20);
            entity.Property(p => p.NombreCompleto).HasColumnName("nombre_completo").IsRequired().HasMaxLength(150);
            entity.Property(p => p.Edad).HasColumnName("edad");
            entity.Property(p => p.Genero).HasColumnName("genero").HasMaxLength(20);
            entity.Property(p => p.Telefono).HasColumnName("telefono").HasMaxLength(25);
            entity.Property(p => p.FechaRegistro).HasColumnName("fecha_registro");
        });

        // 2. Configuración de Sintoma
        modelBuilder.Entity<Sintoma>(entity =>
        {
            entity.ToTable("sintomas");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Id).HasColumnName("id");
            entity.HasIndex(s => s.Codigo).IsUnique();
            entity.Property(s => s.Codigo).HasColumnName("codigo").IsRequired().HasMaxLength(20);
            entity.Property(s => s.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(120);
            entity.Property(s => s.Descripcion).HasColumnName("descripcion").HasMaxLength(300);
            entity.Property(s => s.EsSignoAlarma).HasColumnName("es_signo_alarma");
            entity.Property(s => s.Categoria).HasColumnName("categoria").HasMaxLength(50);
            entity.Property(s => s.NivelGravedadBase).HasColumnName("nivel_gravedad_base");
        });

        // 3. Configuración de ProtocoloAtencion
        modelBuilder.Entity<ProtocoloAtencion>(entity =>
        {
            entity.ToTable("protocolos_atencion");
            entity.HasKey(pr => pr.Id);
            entity.Property(pr => pr.Id).HasColumnName("id");
            entity.Property(pr => pr.NivelPrioridad).HasColumnName("nivel_prioridad");
            entity.Property(pr => pr.CodigoColor).HasColumnName("codigo_color").IsRequired().HasMaxLength(30);
            entity.Property(pr => pr.NombreNivel).HasColumnName("nombre_nivel").IsRequired().HasMaxLength(50);
            entity.Property(pr => pr.TiempoMaximoEsperaMinutos).HasColumnName("tiempo_maximo_espera_minutos");
            entity.Property(pr => pr.AreaDerivacion).HasColumnName("area_derivacion").IsRequired().HasMaxLength(120);
            entity.Property(pr => pr.AccionInmediata).HasColumnName("accion_inmediata").IsRequired().HasMaxLength(300);
        });

        // 4. Configuración de EvaluacionTriage (Relación con Paciente y Protocolo)
        modelBuilder.Entity<EvaluacionTriage>(entity =>
        {
            entity.ToTable("evaluaciones_triage");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PacienteId).HasColumnName("paciente_id");
            entity.Property(e => e.FechaRegistro).HasColumnName("fecha_registro");
            entity.Property(e => e.Temperatura).HasColumnName("temperatura");
            entity.Property(e => e.FrecuenciaCardiaca).HasColumnName("frecuencia_cardiaca");
            entity.Property(e => e.PresionArterial).HasColumnName("presion_arterial").HasMaxLength(20);
            entity.Property(e => e.SaturacionOxigeno).HasColumnName("saturacion_oxigeno");
            entity.Property(e => e.DolorToracico).HasColumnName("dolor_toracico");
            entity.Property(e => e.DificultadRespiratoria).HasColumnName("dificultad_respiratoria");
            entity.Property(e => e.SintomasPrincipales).HasColumnName("sintomas_principales");
            entity.Property(e => e.PrioridadAsignada).HasColumnName("prioridad_asignada");
            entity.Property(e => e.ProtocoloAtencionId).HasColumnName("protocolo_atencion_id");
            entity.Property(e => e.JustificacionRegla).HasColumnName("justificacion_regla").HasMaxLength(500);
            entity.Property(e => e.RequiereAtencionInmediata).HasColumnName("requiere_atencion_inmediata");
            entity.Property(e => e.ObservacionesClinicas).HasColumnName("observaciones_clinicas").HasMaxLength(500);

            entity.HasOne(e => e.Paciente)
                  .WithMany(p => p.Evaluaciones)
                  .HasForeignKey(e => e.PacienteId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Protocolo)
                  .WithMany()
                  .HasForeignKey(e => e.ProtocoloAtencionId)
                  .OnDelete(DeleteBehavior.SetNull);
        });

        // Semillas iniciales (Seed Data - Clase 4 & 6)
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // A. Protocolos Oficiales de Manchester
        modelBuilder.Entity<ProtocoloAtencion>().HasData(
            new ProtocoloAtencion
            {
                Id = 1,
                NivelPrioridad = NivelPrioridad.Resucitacion,
                CodigoColor = "Rojo",
                NombreNivel = "Nivel 1 - Resucitación",
                TiempoMaximoEsperaMinutos = 0,
                AreaDerivacion = "Sala de Shock Trauma / Box de Reanimación",
                AccionInmediata = "Atención médica y de enfermería inmediata continua. Monitoreo invasivo y soporte vital."
            },
            new ProtocoloAtencion
            {
                Id = 2,
                NivelPrioridad = NivelPrioridad.Emergencia,
                CodigoColor = "Naranja",
                NombreNivel = "Nivel 2 - Emergencia",
                TiempoMaximoEsperaMinutos = 10,
                AreaDerivacion = "Urgencias Médicas / Observación Inmediata",
                AccionInmediata = "Evaluación médica en menos de 10 minutos. Monitorización de signos vitales cada 15 min."
            },
            new ProtocoloAtencion
            {
                Id = 3,
                NivelPrioridad = NivelPrioridad.Urgencia,
                CodigoColor = "Amarillo",
                NombreNivel = "Nivel 3 - Urgencia",
                TiempoMaximoEsperaMinutos = 60,
                AreaDerivacion = "Boxes de Consulta Médica Rápida",
                AccionInmediata = "Atención en menos de 60 minutos. Reevaluación periódica de signos si la espera supera 30 min."
            },
            new ProtocoloAtencion
            {
                Id = 4,
                NivelPrioridad = NivelPrioridad.Prioritario,
                CodigoColor = "Verde",
                NombreNivel = "Nivel 4 - Poco Urgente",
                TiempoMaximoEsperaMinutos = 120,
                AreaDerivacion = "Consulta Externa / Triage Secundario",
                AccionInmediata = "Atención ambulatoria antes de 120 minutos. Tratamiento sintomático general."
            },
            new ProtocoloAtencion
            {
                Id = 5,
                NivelPrioridad = NivelPrioridad.NoUrgente,
                CodigoColor = "Azul",
                NombreNivel = "Nivel 5 - No Urgente",
                TiempoMaximoEsperaMinutos = 240,
                AreaDerivacion = "Centro de Salud / Consulta Programada",
                AccionInmediata = "Atención diferida o derivación a consulta externa comunitaria."
            }
        );

        // B. Catálogo Maestro de Síntomas
        modelBuilder.Entity<Sintoma>().HasData(
            new Sintoma
            {
                Id = 1,
                Codigo = "SIN-001",
                Nombre = "Dolor Torácico Opresivo",
                Descripcion = "Dolor retroesternal opresivo con irradiación a mandíbula o extremidad izquierda",
                EsSignoAlarma = true,
                Categoria = "Cardiovascular",
                NivelGravedadBase = 5
            },
            new Sintoma
            {
                Id = 2,
                Codigo = "SIN-002",
                Nombre = "Dificultad Respiratoria Severa (Disnea)",
                Descripcion = "Dificultad severa para ventilar en reposo, estridor o tiraje intercostal",
                EsSignoAlarma = true,
                Categoria = "Respiratorio",
                NivelGravedadBase = 5
            },
            new Sintoma
            {
                Id = 3,
                Codigo = "SIN-003",
                Nombre = "Pérdida Súbita del Estado de Alerta",
                Descripcion = "Síncope, confusión aguda o escala de Glasgow menor a 13",
                EsSignoAlarma = true,
                Categoria = "Neurológico",
                NivelGravedadBase = 5
            },
            new Sintoma
            {
                Id = 4,
                Codigo = "SIN-004",
                Nombre = "Fiebre Elevada (> 38.5 °C)",
                Descripcion = "Registro térmico axilar mayor a 38.5 °C con escalofríos",
                EsSignoAlarma = false,
                Categoria = "Infeccioso",
                NivelGravedadBase = 3
            },
            new Sintoma
            {
                Id = 5,
                Codigo = "SIN-005",
                Nombre = "Cefalea Moderada",
                Descripcion = "Dolor craneal sin signos meníngeos ni focalización motora",
                EsSignoAlarma = false,
                Categoria = "Neurológico",
                NivelGravedadBase = 2
            },
            new Sintoma
            {
                Id = 6,
                Codigo = "SIN-006",
                Nombre = "Trauma Menor en Extremidad",
                Descripcion = "Contusión o esguince en brazo o pierna sin deformidad ósea evidente",
                EsSignoAlarma = false,
                Categoria = "Traumatología",
                NivelGravedadBase = 2
            }
        );

        // C. Pacientes de Demostración
        modelBuilder.Entity<Paciente>().HasData(
            new Paciente
            {
                Id = 1,
                Dni = "0801-1995-12345",
                NombreCompleto = "Juan Carlos Rodríguez",
                Edad = 45,
                Genero = "Masculino",
                Telefono = "+504 9876-5432",
                FechaRegistro = new DateTime(2026, 1, 15, 8, 30, 0, DateTimeKind.Utc)
            },
            new Paciente
            {
                Id = 2,
                Dni = "0801-1950-67890",
                NombreCompleto = "María Elena Castro",
                Edad = 76,
                Genero = "Femenino",
                Telefono = "+504 9911-2233",
                FechaRegistro = new DateTime(2026, 1, 16, 9, 15, 0, DateTimeKind.Utc)
            },
            new Paciente
            {
                Id = 3,
                Dni = "0801-2002-33445",
                NombreCompleto = "Andrés Fernando López",
                Edad = 24,
                Genero = "Masculino",
                Telefono = "+504 9455-6677",
                FechaRegistro = new DateTime(2026, 1, 17, 11, 0, 0, DateTimeKind.Utc)
            }
        );

        // D. Evaluación de Demostración
        modelBuilder.Entity<EvaluacionTriage>().HasData(
            new EvaluacionTriage
            {
                Id = 1,
                PacienteId = 1,
                FechaRegistro = new DateTime(2026, 1, 15, 8, 45, 0, DateTimeKind.Utc),
                Temperatura = 37.2,
                FrecuenciaCardiaca = 115,
                PresionArterial = "140/90",
                SaturacionOxigeno = 88,
                DolorToracico = true,
                DificultadRespiratoria = true,
                SintomasPrincipales = "Dolor Torácico Opresivo, Dificultad Respiratoria Severa",
                PrioridadAsignada = NivelPrioridad.Resucitacion,
                ProtocoloAtencionId = 1,
                JustificacionRegla = "REGLA-01: Dolor torácico + Disnea con SpO2 crítica (<90%). Código Rojo.",
                RequiereAtencionInmediata = true,
                ObservacionesClinicas = "Paciente remitido inmediatamente a Box de Reanimación."
            }
        );
    }
}
