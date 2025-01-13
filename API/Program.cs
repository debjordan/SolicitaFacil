using Microsoft.EntityFrameworkCore;
using SolicitaFacil.API.Middleware;
using SolicitaFacil.Infrastructure.Persistence;
using Microsoft.Extensions.Logging;
using SolicitaFacil.Shared.Services;
using SolicitaFacil.Shared.DTOs.UserDTOs;

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

        app.Run();


    }
}