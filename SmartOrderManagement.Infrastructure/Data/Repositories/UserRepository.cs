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

    public async Task<Guid?> GetCurrentSessionIdAsync(Guid userId)
    {
        return await _context.Users
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(u => u.Id == userId)
            .Select(u => u.CurrentSessionId)
            .FirstOrDefaultAsync();
    }

    public async Task StartNewSessionAsync(Guid userId, Guid sessionId)
    {
        await _context.RefreshTokens
            .IgnoreQueryFilters()
            .Where(rt => rt.UserId == userId && !rt.IsRevoked)
            .ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true));

        await _context.Users
            .IgnoreQueryFilters()
            .Where(u => u.Id == userId)
            .ExecuteUpdateAsync(s => s.SetProperty(u => u.CurrentSessionId, sessionId));
    }
}


