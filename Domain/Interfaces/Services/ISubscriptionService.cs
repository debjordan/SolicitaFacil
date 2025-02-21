using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SolicitaFacil.Domain.Interfaces.Services;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionDetailsDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default);
    Task<SubscriptionDetailsDto> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<CreateSubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto request, CancellationToken cancellationToken = default);
    Task<bool> UpdateSubscriptionByIdAsync(Guid id, UpdateSubscriptionDto request, CancellationToken cancellationToken = default);
    Task<bool> DeleteSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default);
}