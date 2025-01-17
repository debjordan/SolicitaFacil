using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace SolicitaFacil.Application.Services;
public class ValidateService
{
    private readonly ILogger<ValidateService> _logger;

    public ValidateService(ILogger<ValidateService> logger)
    {
        _logger = logger;
    }

    public void ValidateGuid(Guid id, string parameterName)
    {
        if (id == Guid.Empty)
        {
            _logger.LogError("{ParameterName} cannot be empty.", parameterName);
            throw new ValidationException($"{parameterName} cannot be empty.");
        }
    }

    public void ValidateUserDto(object userDto)
    {
        if (userDto == null)
        {
            _logger.LogError("Provided user DTO is null.");
            throw new ValidationException("User data cannot be null.");
        }
    }

    public async void ValidateEmailAtUser(string email)
    {
        if (!new EmailAddressAttribute().IsValid(email))
        {
            _logger.LogError("Invalid email format.");
            throw new Exception("Invalid email format.");
        }

        if (email.Length > 255)
        {
            _logger.LogError("Email length exceeds maximum limit of 255 characters.");
            throw new Exception("Email length exceeds maximum limit of 255 characters.");
        }
    }
}
