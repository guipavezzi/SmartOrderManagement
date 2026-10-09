using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Domain.Entities;public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; } = UserRole.User;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    public Guid? CurrentSessionId { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
