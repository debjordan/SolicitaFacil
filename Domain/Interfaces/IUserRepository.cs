using SolicitaFacil.Domain.Entities;

namespace SolicitaFacil.Domain.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetByIdUserAsync(Guid userId);
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(Guid userId, User user);
    Task DeleteAsync(Guid userId);
    Task EmailExistAsync(string email);
    Task NumberPhoneExistAsync(string phoneNumber);
}
