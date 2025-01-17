using SolicitaFacil.Shared.DTOs.SubscriptionDTOs;

namespace SolicitaFacil.Domain.Interfaces.Services;

public interface ISubscriptionService
{
    Task<IEnumerable<SubscriptionDetailsDto>> GetAllSubscriptionsAsync();
    Task<SubscriptionDetailsDto> GetSubscriptionByIdAsync(Guid Id);
    Task<CreateSubscriptionDto> CreateSubscriptionAsync(CreateSubscriptionDto subscription);
    Task<UpdateSubscriptionDto> UpdateSubscriptionByIdAsync(Guid id, UpdateSubscriptionDto subscription);
    Task<CancelSubscriptionDto> DeleteSubscriptionByIdAsync(Guid id);
}