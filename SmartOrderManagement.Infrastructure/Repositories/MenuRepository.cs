
using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Interfaces.Repositories;
using SmartOrderManagement.Infrastructure.Data.Context;

public class MenuRepository : IMenuRepository
{
    private readonly SmartOrderManagementDbContext _context;

    public MenuRepository(SmartOrderManagementDbContext context)
    {
        _context = context;
    }

    public async Task<Menu> AddAsync(Menu menu)
    {
        await _context.Menus.AddAsync(menu);
        await _context.SaveChangesAsync();
        return menu;
    }

    public async Task DeleteAsync(Menu menu)
    {
        _context.Menus.Remove(menu);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Menu>> GetAllAsync()
    {
        return await _context.Menus.ToListAsync();
    }

    public async Task<Menu?> GetByIdAsync(Guid id)
    {
        return await _context.Menus.Where(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Menu> UpdateAsync(Menu menu)
    {
        _context.Update(menu);
        await _context.SaveChangesAsync();
        return menu;
    }
}
