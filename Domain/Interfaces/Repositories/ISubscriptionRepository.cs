using SolicitaFacil.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolicitaFacil.Domain.Interfaces.Repositories;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Subscription> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Subscription> GetActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(Subscription subscription, CancellationToken cancellationToken = default);
    Task UpdateAsync(Subscription subscription, CancellationToken cancellationToken = default);
}