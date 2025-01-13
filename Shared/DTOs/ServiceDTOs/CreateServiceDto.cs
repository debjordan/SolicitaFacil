using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.API.DTOs.ServiceDTOs;

public class CreateServiceDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal PriceRange { get; set; }
    public string Category { get; set; }
}