namespace SolicitaFacil.Domain.Entities;

public class Subscription
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    
    public bool Status { get; set; }
    public DateTime PayementDate { get; set; }
    public DateTime ExpirationDate { get; set; }
}