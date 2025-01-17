using System.ComponentModel.DataAnnotations;

namespace SolicitaFacil.Shared.DTOs.UserDTOs;
public class CreateUserDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string Password { get; set; }
    public string? PhoneNumber { get; set; }
    public int Age { get; set; }
    public string Role { get; set; }
    public string? SubscriptionPlan { get; set; }
}
