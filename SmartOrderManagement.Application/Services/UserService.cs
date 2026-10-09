using SmartOrderManagement.Application.DTOs.Auth;
using SmartOrderManagement.Application.Interfaces;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums;
using SmartOrderManagement.Domain.Interfaces.Repositories;

namespace SmartOrderManagement.Application.Services;

public class UserService : IUserService
{
    private const int MaxCompanies = 3;

    private readonly IUserRepository _userRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly ITokenService _tokenService;

    public UserService(
        IUserRepository userRepository, 
        ICompanyRepository companyRepository,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _companyRepository = companyRepository;
        _tokenService = tokenService;
    }

    public async Task<TokenResponseDto?> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return null;
        }

        if (await HasReachedCompanyLimitAsync())
        {
            return null;
        }

        var company = new Company
        {
            Name = request.CompanyName
        };
        await _companyRepository.AddAsync(company);

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var sessionId = Guid.NewGuid();

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash,
            CompanyId = company.Id,
            Role = UserRole.Admin,
            CurrentSessionId = sessionId
        };
        await _userRepository.AddAsync(user);

        return await GenerateTokensAsync(user, sessionId);
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null) return null;

        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!isPasswordValid) return null;

        var sessionId = Guid.NewGuid();
        await _userRepository.StartNewSessionAsync(user.Id, sessionId);
        user.CurrentSessionId = sessionId;

        return await GenerateTokensAsync(user, sessionId);
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        var storedToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);
        if (storedToken == null) return null;

        if (storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        await _userRepository.RevokeRefreshTokenAsync(request.RefreshToken);

        var user = storedToken.User;
        var sessionId = user.CurrentSessionId ?? Guid.NewGuid();
        if (user.CurrentSessionId == null)
        {
            await _userRepository.StartNewSessionAsync(user.Id, sessionId);
            user.CurrentSessionId = sessionId;
        }

        return await GenerateTokensAsync(user, sessionId);
    }

    private async Task<TokenResponseDto> GenerateTokensAsync(User user, Guid sessionId)
    {
        var accessToken = _tokenService.GenerateToken(user.Id.ToString(), user.Email, user.CompanyId.ToString(), user.Role, sessionId.ToString());
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };

        await _userRepository.SaveRefreshTokenAsync(refreshToken);

        var company = await _companyRepository.GetByIdAsync(user.CompanyId);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString,
            CompanyName = company?.Name ?? string.Empty
        };
    }

    public async Task<bool> CreateUserAsync(SmartOrderManagement.Application.DTOs.User.CreateUserRequestDto request, Guid companyId)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return false;
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash,
            CompanyId = companyId,
            Role = request.Role
        };

        await _userRepository.AddAsync(user);
        return true;
    }

    private async Task<bool> HasReachedCompanyLimitAsync()
    {
        return await _companyRepository.CountAllAsync() >= MaxCompanies;
    }
}
