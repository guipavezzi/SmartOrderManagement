namespace SmartOrderManagement.Application.Dtos;

public class MenuResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int MinPreparationTimeInMinutes { get; set; }
    public int MaxPreparationTimeInMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
