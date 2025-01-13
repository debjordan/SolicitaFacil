using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.UserDTOs;
public class CreateUserDto
{
    [Required]
    public string? Name { get; set; }
    [Required]
    [EmailAddress]
    public string? Email { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    [Range(0, 120)]
    public int Age { get; set; }
    [Required]
    public string Role { get; set; }
    [Required]
    public string? SubscriptionPlan { get; set; }
}