namespace SolicitaFacil.Domain.Entities;

public class Address
{
    public Guid Id { get; set; }
    public string? Street { get; set; }
    public int Number { get; set; }
    public string? ZipCode { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }

    public ICollection<Service> Services { get; set; } = new List<Service>();

}