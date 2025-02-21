using Microsoft.Extensions.Logging;
using SolicitaFacil.Domain.Entities;
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SolicitaFacil.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(
        ISubscriptionRepository subscriptionRepository,
        ILogger<SubscriptionService> logger)
    {
        _subscriptionRepository = subscriptionRepository ?? throw new ArgumentNullException(nameof(subscriptionRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<SubscriptionDetailsDto>> GetAllSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving all subscriptions.");
        var subscriptions = await _subscriptionRepository.GetAllAsync(cancellationToken);
        return subscriptions.Select(s => MapToDetailsDto(s));
    }

    public async Task<SubscriptionDetailsDto> GetSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Retrieving subscription with ID {SubscriptionId}.", id);
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        return subscription != null ? MapToDetailsDto(subscription) : null;
    }

    public async Task<CreateSubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Creating subscription for user {UserId}.", request.UserId);

        // Verifica se o usuário já tem uma subscrição ativa
        var existingSubscription = await _subscriptionRepository.GetActiveByUserIdAsync(request.UserId, cancellationToken);
        if (existingSubscription != null)
        {
            _logger.LogWarning("Active subscription already exists for user {UserId}.", request.UserId);
            return null;
        }

        var subscription = new Subscription(request.UserId, request.StartDate, request.Amount);
        await _subscriptionRepository.AddAsync(subscription, cancellationToken);

        return new CreateSubscriptionDto
        {
            SubscriptionId = subscription.SubscriptionId,
            UserId = subscription.UserId,
            StartDate = subscription.StartDate,
            Amount = subscription.Amount
        };
    }

    public async Task<bool> UpdateSubscriptionByIdAsync(Guid id, UpdateSubscriptionDto request, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Updating subscription with ID {SubscriptionId}.", id);
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription == null)
        {
            _logger.LogWarning("Subscription with ID {SubscriptionId} not found.", id);
            return false;
        }

        subscription.UpdateAmount(request.Amount);
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        return true;
    }

    public async Task<bool> DeleteSubscriptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Cancelling subscription with ID {SubscriptionId}.", id);
        var subscription = await _subscriptionRepository.GetByIdAsync(id, cancellationToken);
        if (subscription == null)
        {
            _logger.LogWarning("Subscription with ID {SubscriptionId} not found.", id);
            return false;
        }

        subscription.Cancel(DateTime.UtcNow);
        await _subscriptionRepository.UpdateAsync(subscription, cancellationToken);
        return true;
    }

    private SubscriptionDetailsDto MapToDetailsDto(Subscription subscription)
    {
        return new SubscriptionDetailsDto
        {
            SubscriptionId = subscription.SubscriptionId,
            UserId = subscription.UserId,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Status = subscription.Status,
            Amount = subscription.Amount
        };
    }
}