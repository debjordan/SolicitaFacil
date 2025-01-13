using SolicitaFacil.Domain.Entities;
using SolicitaFacil.Domain.Interfaces;
using SolicitaFacil.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SolicitaFacil.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        try
        {
            _logger.LogInformation("Fetching all users from the database.");
            return await _context.Users
                            .AsNoTracking()
                            .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all users.");
            throw;
        }
    }

    public async Task<User> GetByIdUserAsync(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                _logger.LogWarning("Attempt to fetch user with an empty ID.");
                throw new ArgumentException("User ID cannot be empty.");
            }

            var user = await _context.Users
                                .AsNoTracking()
                                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", userId);
                throw new KeyNotFoundException($"User with ID {userId} not found.");
            }

            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving user with ID {UserId}.", userId);
            throw;
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            _logger.LogInformation("Creating a new user with name {UserName}.", user.Name);

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User {UserName} created successfully with ID {UserId}.", user.Name, user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a user.");
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(Guid userId, User user)
    {
        try
        {
            var existingUser = await GetByIdUserAsync(userId);

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User with ID {UserId} updated successfully.", userId);
            return existingUser;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating user with ID {UserId}.", userId);
            throw;
        }
    }

    public async Task DeleteAsync(Guid userId)
    {
        try
        {
            var user = await GetByIdUserAsync(userId);

            _logger.LogInformation("Deleting user with ID {UserId}.", userId);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User with ID {UserId} deleted successfully.", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting user with ID {UserId}.", userId);
            throw;
        }
    }
    public async Task EmailExistAsync(string email)
    {
        var emailExists = await _context.Users
                                    .AnyAsync(u => u.Email == email);

        if (emailExists)
        {
            _logger.LogWarning($"Email {email} already exists.");
            throw new ArgumentException("Email already exists.");
        }
    }

    public async Task NumberPhoneExistAsync(string phone)
    {
        var phoneExists = await _context.Users
                                    .AnyAsync(u => u.PhoneNumber == phone);

        if (phoneExists)
        {
            _logger.LogWarning($"Email {phone} already exists.");
            throw new ArgumentException("Email already exists.");
        }
    }

    public Task PasswordInvalidAsync()
    {
        
        throw new Exception();
    }
}
