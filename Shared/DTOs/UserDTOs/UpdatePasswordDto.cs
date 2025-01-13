using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.UserDTOs;
public class UpdatePasswordDto
{
    [Required]
    public string CurrentPassword { get; set; }

    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}