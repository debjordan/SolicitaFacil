using Microsoft.Extensions.Logging;
using SolicitaFacil.Domain.Interfaces;
using SolicitaFacil.Shared.Services;
using SolicitaFacil.Application.Interfaces;
using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;

namespace SolicitaFacil.Application.Services;

public class SubscriptionService : ISubscriptionService
{
    private readonly ValidateService _validateService;
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly ILogger<SubscriptionService> _logger;
    private readonly IPasswordValidator _passwordValidator;


    public SubscriptionService(ISubscriptionRepository subscriptionRepository, ILogger<SubscriptionService> logger, ValidateService validateService, IPasswordValidator passwordValidator)
    {
        _subscriptionRepository = subscriptionRepository;
        _logger = logger;
        _validateService = validateService;
        _passwordValidator = passwordValidator;
    }
    public async Task<IEnumerable<SubscriptionListDto>> GetAllSubscriptionsAsync()
    {
        _logger.LogInformation("Getting all subscriptions...");
        
        var subscriptions = await _subscriptionRepository.GetAllSubscriptionsAsync();
        if (subscriptions == null)
        {
            _logger.LogWarning("No subscriptions found.");
            return Enumerable.Empty<SubscriptionListDto>();
        }
        return subscriptions.Select(subscriptions => new SubscriptionListDto
        {
            UserId = subscriptions.UserId,
            UserType = subscriptions.UserType,
            PlanName = subscriptions.PlanName,
            PlanPrice = subscriptions.PlanPrice,
            StartDate = subscriptions.StartDate,
            EndDate = subscriptions.EndDate,
            PayementDate = subscriptions.PayementDate,
            ExpirationDate = subscriptions.ExpirationDate,
            Features = subscriptions.Features            
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

    public async Task<SubscriptionDetailsDto> CreateSubscriptionAsync(CreateSubscriptionDto subscriptionDto)
    {
        if (subscriptionDto == null)
        {
            throw new ArgumentNullException("Subscription DTO cannot be null");
        }
        
        var existingsubscription = await _subscriptionRepository.GetSubscriptionByIdAsync(subscriptionDto.UserId);
        if (existingsubscription != null)
        {
            _logger.LogWarning($"User with ID {subscriptionDto.UserId} already has a subscription.");
            return null;
        }

        _logger.LogInformation($"Creating new subscription for user with ID {subscriptionDto.UserId}...");
        var subscription = new CreateSubscriptionDto
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

        var createdSubscription = await _subscriptionRepository.CreateSubscriptionAsync(subscriptionDto);

        _logger.LogInformation($"Subscription created successfully for user with ID {createdSubscription.UserId}.");
        return new SubscriptionDetailsDto
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
}