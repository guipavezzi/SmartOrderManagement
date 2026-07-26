
using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Interfaces.Repositories;
using SmartOrderManagement.Infrastructure.Data.Context;

public class OrderRepository : IOrderRepository
{
    private readonly SmartOrderManagementDbContext _context;
    public OrderRepository(SmartOrderManagementDbContext context)
    {
        _context = context;
    }
    public async Task<Order> AddAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task DeleteAsync(Order order)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync(bool activeOnly = false, bool completedOnly = false, bool includeArchived = false)
    {
        var query = _context.Orders.AsQueryable();

        if (!includeArchived)
        {
            query = query.Where(o => o.ShiftReference == null);
        }

        if (activeOnly)
        {
            query = query.Where(o => o.Status != SmartOrderManagement.Domain.Enums.Status.Completed);
        }
        else if (completedOnly)
        {
            query = query.Where(o => o.Status == SmartOrderManagement.Domain.Enums.Status.Completed);
        }
        return await query.ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _context.Orders.Where(o => o.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Order> UpdateAsync(Order order)
    {
        _context.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<Order> UpdateStatusAsync(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
        return order;
    }
}