namespace SmartOrderManagement.Application.Dtos;

public class CreateMenuRequest
{
    public string Name { get; set; }
    public int MinPreparationTimeInMinutes { get; set; }
    public int MaxPreparationTimeInMinutes { get; set; }
}
