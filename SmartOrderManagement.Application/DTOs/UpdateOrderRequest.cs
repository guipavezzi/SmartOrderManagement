using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Application.Dtos;
public class UpdateOrderRequest
{
    public Guid Id { get; set; }
    public int Table { get; set; }
    public string Ordered { get; set; }
    public string Observation { get; set; }
    public Status Status { get; set; }
}