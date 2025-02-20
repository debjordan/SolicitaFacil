using SolicitaFacil.API.Configuration;
using SolicitaFacil.API.Middleware;

namespace SolicitaFacil.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configurar serviços e logging
        builder.Services.ConfigureServices(builder.Configuration);
        builder.Logging.ConfigureLogging();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}