using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.Application.Interfaces;

public interface ICurrentUserService
{
    Guid UserId { get; }
    Guid CompanyId { get; }
    UserRole Role { get; }
    bool IsAuthenticated { get; }
}