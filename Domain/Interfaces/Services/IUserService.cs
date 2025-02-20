using SolicitaFacil.Shared.DTOs.UserDTOs;

namespace SolicitaFacil.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserListDto>> GetAllUsersAsync();
    Task<UserListDto> GetByIdUserAsync(Guid userId);
    Task<CreateUserDto> CreateUserAsync(CreateUserDto user);
    Task UpdateUserAsync(Guid userId, UpdateUserDto user); // Retorna Task
    Task DeleteUserAsync(Guid userId); // Retorna Task
}