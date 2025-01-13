using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
public class CancelSubscriptionDto
{
    [Required]
    public Guid SubscriptionId { get; set; }
    [Required]
    public Guid UserId { get; set; }
    public Guid UserName { get; set; }
}
