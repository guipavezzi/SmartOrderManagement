using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SmartOrderManagement.Application.Interfaces;
using SmartOrderManagement.Domain.Enums;

namespace SmartOrderManagement.API.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var id = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
    }

    public Guid CompanyId
    {
        get
        {
            var companyId = _httpContextAccessor.HttpContext?.User?.FindFirst("CompanyId")?.Value;
            return string.IsNullOrEmpty(companyId) ? Guid.Empty : Guid.Parse(companyId);
        }
    }

    public UserRole Role
    {
        get
        {
            var roleString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;
            return Enum.TryParse<UserRole>(roleString, out var role) ? role : UserRole.User;
        }
    }

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
}
