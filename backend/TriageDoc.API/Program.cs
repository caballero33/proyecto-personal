using DotNetEnv;
using Microsoft.OpenApi.Models;
using TriageDoc.Application;
using TriageDoc.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno desde el archivo .env local (12-Factor App - Clase 4)
Env.Load();

// 2. Obtener Cadena de Conexión a PostgreSQL (Inyectada desde .env o appsettings)
var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__PostgresConnection")
    ?? builder.Configuration.GetConnectionString("PostgresConnection")
    ?? "Host=localhost;Database=TriageDocDB;Username=admin;Password=SuperSecretPassword123!";

// 3. Inyección de Dependencias por Capas (DIP - Clase 2 y Clase 5)
// Capa de Aplicación: Casos de uso, Motor Experto y Validadores FluentValidation (Clase 8)
builder.Services.AddApplication();

// Capa de Infraestructura: Entity Framework Core, DbContext y Repositorios (Clase 4 y 5)
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
        Description = "API Core y Base de Conocimiento para la UNIDAD 1 (IS-912 Sistemas Expertos). Implementa Clean Architecture, Modelo Relacional de 4 Tablas, Patrón Repositorio y FluentValidation."
    });
});

// Configuración de CORS permisivo para desarrollo y futuro frontend React
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

// 5. Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TriageDoc API v1 (Clean Architecture)");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
