namespace SmartOrderManagement.Application.Dtos;

public class DashboardMetricsDto
{
    public int InPreparation { get; set; }
    public int Attention { get; set; }
    public int Delayed { get; set; }
    public string LongestWaitTime { get; set; }
    public int AveragePreparationTime { get; set; }
}
