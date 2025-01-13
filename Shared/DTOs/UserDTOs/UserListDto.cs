using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.UserDTOs;

public class UserListDto
{
    [Required]
    public Guid UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}