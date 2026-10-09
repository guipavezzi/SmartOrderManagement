using SmartOrderManagement.Application.DTOs.Auth;

namespace SmartOrderManagement.Application.Interfaces;

public interface IUserService
{
    Task<TokenResponseDto?> RegisterAsync(RegisterRequestDto request);
    Task<TokenResponseDto?> LoginAsync(LoginRequestDto request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<bool> CreateUserAsync(SmartOrderManagement.Application.DTOs.User.CreateUserRequestDto request, Guid companyId);
}
