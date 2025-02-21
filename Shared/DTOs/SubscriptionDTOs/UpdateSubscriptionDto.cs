using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
public class UpdateSubscriptionDto
{
    public Guid? SubscriptionId { get; set; }
    public decimal Amount { get; set; }
}
