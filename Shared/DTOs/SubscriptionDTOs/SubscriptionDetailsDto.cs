using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.SubscriptionDTOs;
public class SubscriptionDetailsDto
{
    public Guid SubscriptionId { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; }
    public string UserType { get; set; }
    public string PlanName { get; set; }
    public List<string> Features { get; set; }
    public decimal PlanPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
}