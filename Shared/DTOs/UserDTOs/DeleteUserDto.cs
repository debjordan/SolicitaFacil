using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.UserDTOs;

public class DeleteUserDto
{
    [Required]
    public Guid UserId { get; set; }
}