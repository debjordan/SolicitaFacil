using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolicitaFacil.Domain.Entities;
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Infrastructure.Persistence;

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
        _logger.LogInformation("Fetching all users from the database.");
        return await _context.Users.AsNoTracking().ToListAsync();
    }

    public async Task<User?> GetByIdUserAsync(Guid userId)
    {
        _logger.LogInformation("Fetching user with ID {UserId}", userId);
        return await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<User> CreateUserAsync(User user)
    {
        _logger.LogInformation("Creating user with name {UserName}", user.Name);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateUserAsync(Guid userId, User user)
    {
        _logger.LogInformation("Updating user with ID {UserId}", userId);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid userId)
    {
        _logger.LogInformation("Deleting user with ID {UserId}", userId);
        var user = await GetByIdUserAsync(userId)
            ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EmailExistAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> NumberPhoneExistAsync(string phoneNumber)
    {
        return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
    }
}