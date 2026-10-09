using SmartOrderManagement.Domain.Entities;

namespace SmartOrderManagement.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdAsync(Guid id);
    Task<User> AddAsync(User user);
    Task<int> CountAllAsync();
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task SaveRefreshTokenAsync(RefreshToken refreshToken);
    Task RevokeRefreshTokenAsync(string token);
    Task<Guid?> GetCurrentSessionIdAsync(Guid userId);
    Task StartNewSessionAsync(Guid userId, Guid sessionId);
}
