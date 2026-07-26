using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Domain.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid id);
    Task<IEnumerable<Order>> GetAllAsync(bool activeOnly = false, bool completedOnly = false, bool includeArchived = false);
    Task<Order> AddAsync(Order order);
    Task<Order>UpdateAsync(Order order);
    Task<Order> UpdateStatusAsync(Order order);
    Task DeleteAsync(Order order);
}