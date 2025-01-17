namespace SolicitaFacil.Domain.Entities;

public class Subscription
{
    public Guid UserId { get; set; }
    public User User { get; set; } 
    public string UserType { get; set; }
    public string UserName { get; set; }
    public string PlanName { get; set; }
    public decimal PlanPrice { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime PayementDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public List<string> Features { get; set; }

}
