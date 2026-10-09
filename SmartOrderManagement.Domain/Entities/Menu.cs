namespace SmartOrderManagement.Domain.Entities;

public class Menu
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; }
    public int MinPreparationTimeInMinutes { get; set; }
    public int MaxPreparationTimeInMinutes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public Guid CompanyId { get; set; }
    public Company Company { get; set; }
}
