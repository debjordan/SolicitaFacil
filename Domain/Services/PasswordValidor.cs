using SolicitaFacil.Domain.Interfaces;
using SolicitaFacil.Domain.Interfaces.Services;
namespace SolicitaFacil.Domain.Services;
public class PasswordValidator : IPasswordValidatorService
{
    public bool Validate(string password)
    {
        if (string.IsNullOrEmpty(password) || password.Length < 8)
            return false;

        if (!password.Any(char.IsUpper))
            return false;

        if (!password.Any(char.IsLower))
            return false;

        if (!password.Any(char.IsDigit))
            return false;

        return true;
    }
}
