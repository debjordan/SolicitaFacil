using SolicitaFacil.Shared.DTOs.UserDTOs;
namespace SolicitaFacil.Domain.Interfaces.Services;

public interface IUserService
{
    Task<IEnumerable<UserListDto>> GetAllUsersAsync();
    Task<UserListDto> GetByIdUserAsync(Guid userId);
    Task<CreateUserDto> CreateUserAsync(CreateUserDto user);
    Task<UpdateUserDto> UpdateUserAsync(Guid userId, UpdateUserDto user);
    Task<DeleteUserDto> DeleteUserAsync(Guid userId);
    Task PasswordInvalidAsync(string password);
}
