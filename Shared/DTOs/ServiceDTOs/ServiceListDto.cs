using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.API.DTOs.ServiceDTOs;

public class ServiceListDto
{
    [Required]
    public Guid ServiceId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public decimal PriceRange { get; set; }
}