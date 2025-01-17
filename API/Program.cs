using Microsoft.EntityFrameworkCore;
using SolicitaFacil.API.Middleware;
using SolicitaFacil.Infrastructure.Persistence;
using SolicitaFacil.Application.Services;

namespace SolicitaFacil.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // map endpoints and database configuration
        builder.Services
            .AddEndpointsApiExplorer();
        builder.Services
            .AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(builder.Configuration
            .GetConnectionString("DefaultConnection")));
        
        builder.Services.AddScoped<ValidateService>();
        builder.Services.AddControllers(); // Registrar Controllers
        
        // ILogger
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        var app = builder
            .Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();

        // Mapear Controllers
        app.MapControllers(); // Isso mapeia os controllers para as rotas

        app.Run();
    }
}
