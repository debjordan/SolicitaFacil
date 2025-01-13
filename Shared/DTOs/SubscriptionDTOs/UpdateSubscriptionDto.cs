using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
public class UpdateSubscriptionDto
{
    [Required]
    public Guid SubscriptionId { get; set; }
    public string PlanName { get; set; }
    public decimal PlanPrice { get; set; }
    public List<string> Features { get; set; }
    public DateTime? EndDate { get; set; }
}
