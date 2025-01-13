namespace SolicitaFacil.Domain.Interfaces;

public interface IPasswordValidator
{
    bool Validate(string password);
}
