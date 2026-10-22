using Microsoft.OpenApi.Models;
using ApiEstudiantes.Data;
using ApiEstudiantes.Interfaces;
using ApiEstudiantes.Resources;
using ApiEstudiantes.Services;

var builder = WebApplication.CreateBuilder(args);

const string allowAllCorsPolicy = "AllowAll";

// 1. Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(allowAllCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 2. Registro de Controladores
builder.Services.AddControllers();

// 3. Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Estudiantes",
        Version = "v1",
        Description = "API REST de Gestión de Estudiantes, Profesores, Materias e Inscripciones con arquitectura por capas basada en APIPersonal"
    });
});

#region Inyección de Dependencias (DI)

// Contexto de Conexión a Base de Datos
builder.Services.AddSingleton<DataConnectionContext>();

// Servicio de Respuesta Estándar
builder.Services.AddScoped<IResponseService, ResponseService>();

// Servicios de Negocio y Datos
builder.Services.AddScoped<IServiceEstudiante, ServiceEstudiante>();
builder.Services.AddScoped<IServiceProfesor, ServiceProfesor>();
builder.Services.AddScoped<IServiceMateria, ServiceMateria>();
builder.Services.AddScoped<IServiceEstudianteMateriaRelacion, ServiceEstudianteMateriaRelacion>();

#endregion

var app = builder.Build();

// Configuración del pipeline HTTP
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Estudiantes v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseCors(allowAllCorsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();
