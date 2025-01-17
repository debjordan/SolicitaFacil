using Microsoft.Extensions.Logging;
using SolicitaFacil.Domain.Interfaces.Services;
using SolicitaFacil.Domain.Interfaces.Repositories;
using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
using SolicitaFacil.Domain.Services;
using SolicitaFacil.Domain.Entities;

namespace SolicitaFacil.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ValidateService _validateService;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscriptionService> _logger;
    private readonly PasswordValidator _passwordValidator;


    public SubscriptionService(ISubscriptionRepository subscriptionRepository, ILogger<SubscriptionService> logger, ValidateService validateService, PasswordValidator passwordValidator)
    {
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
        _validateService = validateService;
        _passwordValidator = passwordValidator;
    }
    public async Task<IEnumerable<SubscriptionDetailsDto>> GetAllSubscriptionsAsync()
    {
        _logger.LogInformation("Getting all subscriptions...");
        
        var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
        if (subscriptions == null)
        {
            _logger.LogWarning("No subscriptions found.");
            return Enumerable.Empty<SubscriptionDetailsDto>();
        }
        return subscriptions.Select(subscription => new SubscriptionDetailsDto
        {
            UserId = subscription.UserId,
            UserType = subscription.UserType,
            PlanName = subscription.PlanName,
            PlanPrice = subscription.PlanPrice,

            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Features = subscription.Features
        });
    }

    public async Task<SubscriptionDetailsDto> GetSubscriptionByIdAsync(Guid id)
    {
        _logger.LogInformation($"Getting subscription with ID {id}...");
        var subscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
        if (subscription == null)
        {
            _logger.LogWarning($"No subscription found with ID {id}.");
            return null;
        }
        return new SubscriptionDetailsDto
        {
            UserId = subscription.UserId,
            UserName = subscription.UserName,
            UserType = subscription.UserType,
            PlanName = subscription.PlanName,
            PlanPrice = subscription.PlanPrice,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Features = subscription.Features
        };
    }

    public async Task<CreateSubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto subscriptionDto)
    {
        if (subscriptionDto == null)
        {
            throw new ArgumentNullException("Subscription DTO cannot be null");
        }

        // Verifique se a assinatura já existe no repositório
        var existingsubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionDto.UserId);
        if (existingsubscription != null)
        {
            _logger.LogWarning($"User with ID {subscriptionDto.UserId} already has a subscription.");
            return null;
        }

        _logger.LogInformation($"Creating new subscription for user with ID {subscriptionDto.UserId}...");

        // Crie a entidade Subscription a partir do DTO
        var subscription = new Subscription
        {
            UserId = subscriptionDto.UserId,
            UserType = subscriptionDto.UserType,
            PlanName = subscriptionDto.PlanName,
            PlanPrice = subscriptionDto.PlanPrice,
            StartDate = subscriptionDto.StartDate,
            EndDate = subscriptionDto.EndDate,
            PayementDate = subscriptionDto.PayementDate,
            ExpirationDate = subscriptionDto.ExpirationDate,
            Features = subscriptionDto.Features
        };

        // Agora você pode passar a entidade Subscription para o repositório
        var createdSubscription = await _subscriptionRepository.CreateSubscriptionAsync(subscription);

        _logger.LogInformation($"Subscription created successfully for user with ID {createdSubscription.UserId}.");

        // Retorne o DTO com os detalhes da nova assinatura criada
        return new CreateSubscriptionDto
        {
            UserId = createdSubscription.UserId,
            UserType = createdSubscription.UserType,
            PlanName = createdSubscription.PlanName,
            PlanPrice = createdSubscription.PlanPrice,
            StartDate = createdSubscription.StartDate,
            EndDate = createdSubscription.EndDate,
            Features = createdSubscription.Features
        };
    }
    public async Task<UpdateSubscriptionDto> UpdateSubscriptionByIdAsync(Guid id, UpdateSubscriptionDto subscriptionDto)
    {
        if (subscriptionDto == null)
        {
            throw new ArgumentNullException("Subscription DTO cannot be null");
        }

        var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
        if (existingSubscription == null)
        {
            _logger.LogWarning($"No subscription found with ID {id}.");
            return null;
        }

        existingSubscription.PlanName = subscriptionDto.PlanName;
        existingSubscription.PlanPrice = subscriptionDto.PlanPrice;
        existingSubscription.Features = subscriptionDto.Features;

        var updatedSubscription = await _subscriptionRepository.UpdateSubscriptionByIdAsync(id, existingSubscription);

        _logger.LogInformation($"Subscription with ID {id} updated successfully.");

        return new UpdateSubscriptionDto
        {
            PlanName = updatedSubscription.PlanName,
            PlanPrice = updatedSubscription.PlanPrice,
            EndDate = updatedSubscription.EndDate,
            Features = updatedSubscription.Features
        };
    }

    public async Task<CancelSubscriptionDto> DeleteSubscriptionByIdAsync(Guid id)
    {
        var existingSubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(id);
        if (existingSubscription == null)
        {
            _logger.LogWarning($"No subscription found with ID {id}.");
            return null;
        }

        await _subscriptionRepository.DeleteSubscriptionByIdAsync(id);

        _logger.LogInformation($"Subscription with ID {id} deleted successfully.");

        return new CancelSubscriptionDto
        {
            UserId = existingSubscription.UserId,
        };
    }


}