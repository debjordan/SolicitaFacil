using SolicitaFacil.Domain.Entities;
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Shared.DTOs.UserDTOs;
using Microsoft.Extensions.Logging;

namespace SolicitaFacil.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserService> _logger;
    private readonly IPasswordValidatorService _passwordValidator;

    public UserService(
        IUserRepository userRepository,
        ILogger<UserService> logger,
        IPasswordValidatorService passwordValidatorService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _passwordValidator = passwordValidatorService;
    }

    public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("Fetching all users...");
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(u => new UserListDto
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            PhoneNumber = u.PhoneNumber,
            Role = u.Role
        });
    }

    public async Task<UserListDto> GetByIdUserAsync(Guid userId)
    {
        _logger.LogInformation("Fetching user with ID {UserId}", userId);
        var user = await _userRepository.GetByIdUserAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        return new UserListDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        };
    }

    public async Task<CreateUserDto> CreateUserAsync(CreateUserDto userDto)
    {
        _logger.LogInformation("Creating new user with email {Email}", userDto.Email);

        if (await _userRepository.EmailExistAsync(userDto.Email))
            throw new InvalidOperationException("Email is already in use.");
        if (await _userRepository.NumberPhoneExistAsync(userDto.PhoneNumber))
            throw new InvalidOperationException("Phone number is already in use.");
        if (!_passwordValidator.Validate(userDto.Password))
            throw new ArgumentException("Password does not meet the required standards.");

        var user = new User(
            name: userDto.Name,
            email: userDto.Email,
            password: BCrypt.Net.BCrypt.HashPassword(userDto.Password),
            phoneNumber: userDto.PhoneNumber,
            role: userDto.Role
        );

        await _userRepository.CreateUserAsync(user);
        _logger.LogInformation("User {Name} created successfully with ID {Id}", user.Name, user.Id);

        return new CreateUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        };
    }

    public async Task UpdateUserAsync(Guid userId, UpdateUserDto userDto)
    {
        _logger.LogInformation("Updating user with ID {UserId}", userId);
        var user = await _userRepository.GetByIdUserAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");

        user.Update(
            name: userDto.Name,
            email: userDto.Email,
            phoneNumber: userDto.PhoneNumber,
            role: userDto.Role
        );

        await _userRepository.UpdateUserAsync(userId, user);
        _logger.LogInformation("User {Id} updated successfully", userId);
    }

    public async Task DeleteUserAsync(Guid userId)
    {
        _logger.LogInformation("Deleting user with ID {UserId}", userId);
        var user = await _userRepository.GetByIdUserAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        await _userRepository.DeleteAsync(userId);
        _logger.LogInformation("User {Id} deleted successfully", userId);
    }
}