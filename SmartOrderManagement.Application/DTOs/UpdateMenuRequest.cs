namespace SmartOrderManagement.Application.Dtos;

public class UpdateMenuRequest
{
    public string Name { get; set; }
    public int MinPreparationTimeInMinutes { get; set; }
    public int MaxPreparationTimeInMinutes { get; set; }
}
