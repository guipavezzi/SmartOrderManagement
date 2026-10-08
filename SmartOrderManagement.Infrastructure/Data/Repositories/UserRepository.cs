using Microsoft.EntityFrameworkCore;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Interfaces.Repositories;
using SmartOrderManagement.Infrastructure.Data.Context;

namespace SmartOrderManagement.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SmartOrderManagementDbContext _context;

    public UserRepository(SmartOrderManagementDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<User> AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<int> CountAllAsync()
    {
        // Ignora o filtro de tenant para contar os usuários do sistema inteiro
        return await _context.Users.IgnoreQueryFilters().CountAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens
            .Include(rt => rt.User).IgnoreQueryFilters().FirstOrDefaultAsync(rt => rt.Token == token);
    }

    public async Task SaveRefreshTokenAsync(RefreshToken refreshToken)
    {
        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
    }

    public async Task RevokeRefreshTokenAsync(string token)
    {
        var rt = await _context.RefreshTokens.IgnoreQueryFilters().FirstOrDefaultAsync(t => t.Token == token);
        if (rt != null)
        {
            rt.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }
}


