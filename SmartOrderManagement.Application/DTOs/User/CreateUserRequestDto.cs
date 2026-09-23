using System.ComponentModel.DataAnnotations;
using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Application.DTOs.User;

public class CreateUserRequestDto
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

    public UserRole Role { get; set; } = UserRole.User;
}
