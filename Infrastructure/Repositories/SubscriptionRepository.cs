using Microsoft.EntityFrameworkCore;
using SolicitaFacil.Domain.Entities;
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolicitaFacil.Infrastructure.Repositories;

public class SubscriptionRepository : ISubscriptionRepository
{
    private readonly AppDbContext _context;

    public SubscriptionRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Subscription>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Subscriptions
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.SubscriptionId == id, cancellationToken);
    }

    public async Task<Subscription> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Subscriptions
            .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == "Active", cancellationToken);
    }

    public async Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        _context.Subscriptions.Add(subscription);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default)
    {
        _context.Subscriptions.Update(subscription);
        await _context.SaveChangesAsync(cancellationToken);
    }
}