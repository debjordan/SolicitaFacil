using System.Numerics;

namespace SolicitaFacil.Domain.Entities;
public class User
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Email { get; set; }
    public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
    public string? PhoneNumber { get; set; }
    public string Role { get; set; }
    public DateTime CreatedAt { get; set; } 
    
    public ICollection<Service> Services { get; set; } = new List<Service>();
    public Subscription Subscription { get; set; }
}