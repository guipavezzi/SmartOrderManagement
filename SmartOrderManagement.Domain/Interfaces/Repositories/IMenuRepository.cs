using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Domain.Interfaces.Repositories;

public interface IMenuRepository
{
    Task<Menu?> GetByIdAsync(Guid id);
    Task<IEnumerable<Menu>> GetAllAsync();
    Task<Menu> AddAsync(Menu menu);
    Task<Menu> UpdateAsync(Menu menu);
    Task DeleteAsync(Menu menu);
}
