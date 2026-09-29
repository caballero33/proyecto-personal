using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using TriageDoc.Application.Interfaces;
using TriageDoc.Application.Services;
using TriageDoc.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Cargar variables de entorno desde el archivo .env local (12-Factor App - Clase 4)
Env.Load();

// 2. Configurar servicios en el contenedor de dependencias (DIP - Clase 2)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Inyección de dependencias del Motor Experto (SOLID DIP)
builder.Services.AddScoped<ITriageService, TriageService>();

// Configuración de Entity Framework Core con PostgreSQL (Clase 4)
var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<TriageDocDbContext>(options =>
        options.UseNpgsql(connectionString));
}

var app = builder.Build();

// 3. Configuración del Pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TriageDoc API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
