using SmartOrderManagement.Application.Dtos;
using SmartOrderManagement.Domain.Enums;

public interface IOrderService
{
    Task<OrderResponse> CreateOrderAsync(CreateOrderRequest request);
    Task<OrderResponse?> GetOrderByIdAsync(Guid id);
    Task<IEnumerable<OrderResponse>> GetOrdersAsync(bool activeOnly = false, bool completedOnly = false, bool includeArchived = false);
    Task<OrderResponse> UpdateOrderAsync(UpdateOrderRequest request, Guid id);
    Task<bool> CancelOrderAsync(Guid id);
    Task<DashboardMetricsDto> GetMetricsAsync();
    Task<DashboardAnalyticsDto> GetAnalyticsAsync();
    Task<string> CloseShiftAsync();

    Task<bool> UpdateOrderStatusAsync(Guid id, Status status);
}