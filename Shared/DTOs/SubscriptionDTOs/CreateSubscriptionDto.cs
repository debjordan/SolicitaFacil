using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.SubscriptionDTOs;

public class CreateSubscriptionDto
{
    [Required]
    public Guid UserId { get; set; }
    public string UserType { get; set; }
    public string PlanName { get; set; }
    public decimal PlanPrice { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; }
    public DateTime PayementDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<string> Features { get; set; }
}