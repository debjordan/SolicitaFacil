using System;

namespace SolicitaFacil.Domain.Entities;

public class Subscription
{
    public Guid SubscriptionId { get; private set; }
    public Guid UserId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public string Status { get; private set; } // Ex.: "Active", "Cancelled", "Expired"
    public decimal Amount { get; private set; } // Valor da subscrição (se aplicável)

    // Construtor privado para EF Core
    private Subscription() { }

    public Subscription(Guid userId, DateTime startDate, decimal amount)
    {
        SubscriptionId = Guid.NewGuid();
        UserId = userId;
        StartDate = startDate;
        EndDate = null;
        Status = "Active";
        Amount = amount;

        Validate();
    }

    // Métodos de negócio
    public void Cancel(DateTime cancellationDate)
    {
        if (Status != "Active")
            throw new InvalidOperationException("Only active subscriptions can be cancelled.");

        if (cancellationDate < StartDate)
            throw new InvalidOperationException("Cancellation date cannot be earlier than start date.");

        Status = "Cancelled";
        EndDate = cancellationDate;
    }

    public void UpdateAmount(decimal newAmount)
    {
        if (newAmount < 0)
            throw new InvalidOperationException("Amount cannot be negative.");

        Amount = newAmount;
    }

    private void Validate()
    {
        if (UserId == Guid.Empty)
            throw new ArgumentException("UserId is required.");
        if (StartDate < DateTime.UtcNow.Date)
            throw new ArgumentException("StartDate cannot be in the past.");
        if (Amount < 0)
            throw new ArgumentException("Amount cannot be negative.");
    }
}