using SolicitaFacil.Domain.Interfaces.Services;

namespace SolicitaFacil.Application.Services;

public class PasswordValidatorService : IPasswordValidatorService
{
    public bool Validate(string password)
    {
        if (string.IsNullOrWhiteSpace(password)) return false;
        if (password.Length < 8) return false;
        if (!password.Any(char.IsUpper)) return false;
        if (!password.Any(char.IsLower)) return false;
        if (!password.Any(char.IsDigit)) return false;
        if (!password.Any(ch => "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~".Contains(ch))) return false;

        return true;
    }
}