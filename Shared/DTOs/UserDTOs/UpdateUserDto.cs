using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.UserDTOs;
public class UpdateUserDto
{
    [Required]
    public Guid UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Name { get; set; }
    [Range(0, 120)]
    public int Age { get; set; }
    public string? Role { get; set; }
}