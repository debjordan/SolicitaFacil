namespace SolicitaFacil.Domain.Interfaces.Services;

public interface IPasswordValidatorService
{
    bool Validate(string password);
}