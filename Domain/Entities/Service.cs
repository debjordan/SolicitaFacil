namespace SolicitaFacil.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PriceRange { get; set; }
    public string Category { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid? AddressId { get; set; }
    public Address Address { get; set; }
}