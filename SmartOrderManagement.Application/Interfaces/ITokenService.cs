using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(string userId, string email, string companyId, UserRole role, string sessionId);
    string GenerateRefreshToken();
}
