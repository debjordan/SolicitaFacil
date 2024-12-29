using Microsoft.EntityFrameworkCore;
using SolicitaFacil.Infrastructure.Persistence;

namespace SolicitaFacil.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddEndpointsApiExplorer();
        builder.Services
            .AddSwaggerGen();
        builder.Services.AddDbContext<AppDbContext>(options => options
            .UseSqlServer(builder.Configuration
            .GetConnectionString("DefaultConnection")));

        var app = builder
            .Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.Run();


    }
}