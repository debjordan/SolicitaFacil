using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.API.DTOs.ServiceDTOs;

public class DeleteServiceDto
{
    [Required]
    public Guid ServiceId { get; set; }
}