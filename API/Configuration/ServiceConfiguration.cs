using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using SolicitaFacil.Infrastructure.Persistence;
using SolicitaFacil.Domain.Interfaces.Services; // Para IUserService e IPasswordValidatorService
using SolicitaFacil.Application.Services; // Para UserService e PasswordValidatorService
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Infrastructure.Repositories;
using SolicitaFacil.Shared.Validators;
using Microsoft.Extensions.Configuration;

namespace SolicitaFacil.API.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Banco de Dados
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositórios
        services.AddScoped<IUserRepository, UserRepository>();

        // Serviços
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordValidatorService, PasswordValidatorService>();

        // Controllers e FluentValidation
        services.AddControllers()
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateUserDtoValidator>());

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder logging)
    {
        logging.ClearProviders();
        logging.AddConsole();
        return logging;
    }
}