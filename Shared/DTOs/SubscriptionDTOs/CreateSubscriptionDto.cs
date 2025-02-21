using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.SubscriptionDTOs;

public class CreateSubscriptionDto
{
    public Guid? SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartDate { get; set; }
    public decimal Amount { get; set; }
}