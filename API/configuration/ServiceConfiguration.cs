using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SolicitaFacil.Application.Services;
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Infrastructure.Persistence;
using SolicitaFacil.Infrastructure.Repositories;
using SolicitaFacil.Shared.Validators;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace SolicitaFacil.API.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Banco de Dados
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlOptions => sqlOptions.EnableRetryOnFailure()));

        // Repositórios
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();

        // Serviços
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IPasswordValidatorService, PasswordValidatorService>();
        services.AddScoped<ISubscriptionService, SubscriptionService>();

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()   // Permite qualquer origem (útil em dev)
                       .AllowAnyMethod()   // Permite qualquer método HTTP (GET, POST, etc.)
                       .AllowAnyHeader();  // Permite qualquer header

                // Para produção, ajuste para algo mais restritivo, como:
                // builder.WithOrigins("https://seusite.com")
                //        .WithMethods("GET", "POST", "PUT", "DELETE")
                //        .WithHeaders("Authorization", "Content-Type");
            });
        });

        // Controllers e FluentValidation
        services.AddControllers(options =>
        {
            options.Filters.Add<ExceptionFilter>(); // Filtro global de exceções
        })
        .AddFluentValidation(fv =>
        {
            fv.RegisterValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
            fv.RegisterValidatorsFromAssemblyContaining<CreateSubscriptionDtoValidator>();
        });

        // Versionamento de API
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        // Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "SolicitaFacil API", Version = "v1" });
            c.EnableAnnotations(); // Suporte a atributos Swagger
        });

        // Logging
        services.AddLogging();

        return services;
    }

    public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder logging, IConfiguration configuration)
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.AddDebug();
        logging.AddConfiguration(configuration.GetSection("Logging"));
        return logging;
    }
}

// Filtro de exceção global
public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "An unhandled exception occurred.");
        var response = new ApiResponse<object>(false, "An internal error occurred.", null, context.Exception.Message);
        context.Result = new ObjectResult(response)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        context.ExceptionHandled = true;
    }
}