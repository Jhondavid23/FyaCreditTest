using FyaCreditManagement.IOC;
using FyaCreditManagement.IOC.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =============================================
// CONFIGURACIÓN DE CORS
// =============================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",    // Vite dev server
                "http://localhost:3000",    // React dev server alternativo
                "https://localhost:5173",   // HTTPS version
                "https://localhost:3000"    // HTTPS version
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials(); // Permite cookies y headers de autenticación
    });

    // Política más permisiva para desarrollo
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Inyectar dependencias 
builder.Services.InyectarDependencias(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Usar CORS permisivo en desarrollo
    app.UseCors("Development");
}
else
{
    // Usar CORS específico en producción
    app.UseCors("AllowReactApp");
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();