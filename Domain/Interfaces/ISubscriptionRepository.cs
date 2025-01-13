using SolicitaFacil.Domain.Entities;
namespace SolicitaFacil.Domain.Interfaces;

public interface ISubscriptionRepository
{
    Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync();
    Task<Subscription> GetSubscriptionByIdAsync(Guid Id);
    Task<Subscription> CreateSubscriptionAsync(Subscription request);
    Task<Subscription> UpdateSubscriptionByIdAsync(Guid id, Subscription request);
    Task<Subscription> DeleteSubscriptionByIdAsync(Guid id);
}