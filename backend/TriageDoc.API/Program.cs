using DotNetEnv;
using Microsoft.OpenApi.Models;
using TriageDoc.Application;
using TriageDoc.Infrastructure;
using TriageDoc.Infrastructure.Data;

// Compatibilidad de DateTime UTC con columnas TIMESTAMP en PostgreSQL
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno desde el archivo .env local si existe (12-Factor App - Clase 4)
Env.Load();

// 2. Obtener Cadena de Conexión a PostgreSQL (Inyectada desde Docker Compose, .env o appsettings)
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection")
    ?? builder.Configuration.GetConnectionString("PostgresConnection")
    ?? "Host=localhost;Database=TriageDocDB;Username=admin;Password=SuperSecretPassword123!";

// 3. Inyección de Dependencias por Capas (DIP - Clase 2 y Clase 5)
builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);

// Capa de Presentación: Controladores REST y Serialización JSON
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 4. Configuración de Documentación OpenAPI / Swagger (Clase 7)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TriageDoc API - Sistema Experto de Triage Médico",
        Version = "v1",
        Description = "API Core y Base de Conocimiento (IS-912 Sistemas Expertos). Implementa Clean Architecture, Modelo Relacional de 4 Tablas, Patrón Repositorio y FluentValidation."
    });
});

// Configuración de CORS permisivo para comunicación con cliente React / Nginx
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Asegurar que el esquema y datos semilla existan al iniciar el contenedor
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<TriageDocDbContext>();
        db.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[TriageDoc.API] Aviso al verificar BD inicial: {ex.Message}");
    }
}

// 5. Configuración del Pipeline HTTP (Habilitado en Desarrollo y Contenedor Docker)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TriageDoc API v1 (Clean Architecture)");
    c.RoutePrefix = "swagger";
});

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
