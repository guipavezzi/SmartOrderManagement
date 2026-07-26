namespace SmartOrderManagement.Domain.Entities;

using SmartOrderManagement.Domain.Enums;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int Table { get; set; }
    public string Ordered { get; set; }
    public DateTime? Time { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Status Status { get; set; } = Status.InPreparation;
    public string Observation { get; set; }
    public string? ShiftReference { get; set; }
}