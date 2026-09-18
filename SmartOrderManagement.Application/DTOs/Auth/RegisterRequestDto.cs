using System.ComponentModel.DataAnnotations;

namespace SmartOrderManagement.Application.DTOs.Auth;

public class RegisterRequestDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string CompanyName { get; set; } = string.Empty;
}
