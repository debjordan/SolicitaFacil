using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Shared.DTOs.UserDTOs;
using SolicitaFacil.Domain.Entities;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Application.Services;

public class UserService : IUserService
{
    private readonly IPasswordValidatorService _passwordValidator;

    private readonly ValidateService _validateService;
    private readonly IUserRepository _userRepository;

    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, ILogger<UserService> logger, ValidateService validateService, IPasswordValidatorService passwordValidatorService)
    {
        _userRepository = userRepository;
        _logger = logger;
        _validateService = validateService;
        _passwordValidator = passwordValidatorService;
    }

    public async Task<IEnumerable<UserListDto>> GetAllUsersAsync()
    {
        _logger.LogInformation("Getting all users...");

        var users = await _userRepository.GetAllUsersAsync();
        if (users == null)
        {
            _logger.LogWarning("No users found.");
            return Enumerable.Empty<UserListDto>();
        }
        return users.Select(users => new UserListDto
        {
            Name = users.Name,
            Email = users.Email,
            PhoneNumber = users.PhoneNumber,
            Role = users.Role
        });
    }

    public async Task<UserListDto> GetByIdUserAsync(Guid userId)
    {
        _validateService.ValidateGuid(userId, nameof(userId));

        _logger.LogInformation("Fetching user with ID {UserId}");

        var user = await _userRepository.GetByIdUserAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found.");
            throw new Exception($"User with ID {userId} not found.");
        }

        return new UserListDto
        {
            Name = user.Name,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber,
            Role = user.Role
        };
    }

    public async Task<CreateUserDto> CreateUserAsync(CreateUserDto userDto)
    {
        if (userDto == null)
        {
            throw new ArgumentNullException(nameof(userDto), "User DTO cannot be null");
        }

        if (string.IsNullOrEmpty(userDto.Email))
        {
            throw new ArgumentException("Email is required.");
        }

        if (string.IsNullOrEmpty(userDto.PhoneNumber))
        {
            throw new ArgumentException("Phone number is required.");
        }

        if (!_passwordValidator.Validate(userDto.Password))
        {
            throw new ArgumentException("Password does not meet the required standards.");
        }

        var existingEmail = await _userRepository.EmailExistAsync(userDto.Email);
        if (existingEmail)
        {
            throw new ArgumentException("Email is already in use.");
        }

        var existingPhoneNumber = await _userRepository.NumberPhoneExistAsync(userDto.PhoneNumber);
        if (existingPhoneNumber)
        {
            throw new ArgumentException("Phone number is already in use.");
        }

        var hashedPassword = HashPassword(userDto.Password);

        _logger.LogInformation("Creating new user...");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Role = userDto.Role,
            Password = hashedPassword 
        };

        var createdUser = await _userRepository.CreateUserAsync(user);

        _logger.LogInformation($"User {user.Name} created successfully with ID {user.Id}");

        return new CreateUserDto
        {
            Name = createdUser.Name,
            Email = createdUser.Email,
            PhoneNumber = createdUser.PhoneNumber,
            Role = createdUser.Role
        };
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public async Task<UpdateUserDto> UpdateUserAsync(Guid userId, UpdateUserDto userDto)
    {
        _validateService.ValidateGuid(userId, nameof(userId));
        if (userDto == null)
        {
            throw new ValidationException("UserDto cannot be null");
        }

        var user = await _userRepository.GetByIdUserAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found.");
            throw new Exception($"User with ID {userId} not found.");
        }

        _logger.LogInformation($"Updating user with ID {userId}");

        user.Name = userDto.Name;
        user.Email = userDto.Email;
        user.PhoneNumber = userDto.PhoneNumber;
        user.Role = userDto.Role;

        var updatedUser = await _userRepository.UpdateUserAsync(userId, user);

        if (updatedUser == null)
        {
            _logger.LogWarning("Failed to update user.");
            throw new Exception($"Failed to update user with ID {userId}.");
        }

        _logger.LogInformation($"User {updatedUser.Id} updated successfully.");

        return new UpdateUserDto
        {
            Name = updatedUser.Name,
            Email = updatedUser.Email,
            PhoneNumber = updatedUser.PhoneNumber,
            Role = updatedUser.Role
        };
    }

    public async Task<DeleteUserDto> DeleteUserAsync(Guid userId)
    {
        _validateService.ValidateGuid(userId, nameof(userId));

        var user = await _userRepository.GetByIdUserAsync(userId);
        if (user == null)
        {
            _logger.LogWarning("User not found.");
            throw new Exception($"User with ID {userId} not found.");
        }
    
        _logger.LogInformation($"Deleting user with ID {userId}");
        await _userRepository.DeleteAsync(userId);

        _logger.LogInformation("User with ID {UserId} deleted successfully.", userId);

        return new DeleteUserDto
        {
        };
    }
    public async Task PasswordInvalidAsync(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("Password cannot be null or empty.");
        }

        if (password.Length < 8)
        {
            throw new Exception("Password must be at least 8 characters long.");
        }

        if (!password.Any(char.IsUpper))
        {
            throw new Exception("Password must contain at least one uppercase letter.");
        }

        if (!password.Any(char.IsLower))
        {
            throw new Exception("Password must contain at least one lowercase letter.");
        }

        if (!password.Any(char.IsDigit))
        {
            throw new Exception("Password must contain at least one numeric digit.");
        }

        if (!password.Any(ch => "!@#$%^&*()-_=+[]{}|;:'\",.<>?/`~".Contains(ch)))
        {
            throw new Exception("Password must contain at least one special character.");
        }
        await Task.CompletedTask;
    }
}
