using Microsoft.EntityFrameworkCore;
using SolicitaFacil.Domain.Entities;
using SolicitaFacil.Domain.Interfaces.Repositories;

namespace SolicitaFacil.Infrastructure.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly DbContext _context;

        public SubscriptionRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subscription>> GetAllSubscriptionsAsync()
        {
            return await _context.Set<Subscription>().ToListAsync();
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(Guid id)
        {
            return await _context.Set<Subscription>().FirstOrDefaultAsync(s => s.UserId == id);
        }

        public async Task<Subscription> CreateSubscriptionAsync(Subscription request)
        {
            var subscription = new Subscription
            {
                UserId = request.UserId,
                UserType = request.UserType,
                PlanName = request.PlanName,
                PlanPrice = request.PlanPrice,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PayementDate = request.PayementDate,
                ExpirationDate = request.ExpirationDate,
                Features = request.Features
            };

            await _context.Set<Subscription>().AddAsync(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }

        public async Task<Subscription> UpdateSubscriptionByIdAsync(Guid id, Subscription request)
        {
            var existingSubscription = await _context.Set<Subscription>().FirstOrDefaultAsync(s => s.UserId == id);
            if (existingSubscription == null)
            {
                return null;
            }

            existingSubscription.PlanName = request.PlanName;
            existingSubscription.PlanPrice = request.PlanPrice;
            existingSubscription.StartDate = request.StartDate;
            existingSubscription.EndDate = request.EndDate;
            existingSubscription.Features = request.Features;

            await _context.SaveChangesAsync();
            return existingSubscription;
        }

        public async Task<Subscription> DeleteSubscriptionByIdAsync(Guid id)
        {
            var subscription = await _context.Set<Subscription>().FirstOrDefaultAsync(s => s.UserId == id);
            if (subscription == null)
            {
                return null;
            }

            _context.Set<Subscription>().Remove(subscription);
            await _context.SaveChangesAsync();

            return subscription;
        }
    }
}
