namespace SmartOrderManagement.Application.Dtos;

public class DashboardAnalyticsDto
{
    public int AveragePreparationTime { get; set; }
    public double EfficiencyRate { get; set; }
    public int TotalCompletedOrders { get; set; }
    public int MostActiveTable { get; set; }
    public Dictionary<string, int> TopDishes { get; set; } = new();
    public Dictionary<string, int> PeakHours { get; set; } = new();
}
