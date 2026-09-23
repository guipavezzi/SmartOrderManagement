using SmartOrderManagement.Application.DTOs.Auth;
using SmartOrderManagement.Application.Interfaces;
using SmartOrderManagement.Domain.Entities;
using SmartOrderManagement.Domain.Enums;
using SmartOrderManagement.Domain.Interfaces.Repositories;

namespace SmartOrderManagement.Application.Services;

public class UserService : IUserService
{
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
            // Email already in use
            return null;
        }

        // Create the company first
        var company = new Company
        {
            Name = request.CompanyName
        };
        await _companyRepository.AddAsync(company);

        // Hash the password
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // Create the user as Admin for this new company
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = passwordHash,
            CompanyId = company.Id,
            Role = UserRole.Admin // First user is the admin/manager of the SaaS
        };
        await _userRepository.AddAsync(user);

        // Generate tokens
        return await GenerateTokensAsync(user);
    }

    public async Task<TokenResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null) return null;

        // Verify password
        bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.Password);
        if (!isPasswordValid) return null;

        // Generate tokens
        return await GenerateTokensAsync(user);
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        // 1. Find the refresh token in the DB
        var storedToken = await _userRepository.GetRefreshTokenAsync(request.RefreshToken);
        if (storedToken == null) return null;

        // 2. Check if it's valid
        if (storedToken.IsRevoked || storedToken.ExpiresAt <= DateTime.UtcNow)
        {
            return null;
        }

        // 3. Invalidate the old token
        await _userRepository.RevokeRefreshTokenAsync(request.RefreshToken);

        // 4. Generate new tokens for the user
        var user = storedToken.User;
        return await GenerateTokensAsync(user);
    }

    private async Task<TokenResponseDto> GenerateTokensAsync(User user)
    {
        var accessToken = _tokenService.GenerateToken(user.Id.ToString(), user.Email, user.CompanyId.ToString(), user.Role);
        var refreshTokenString = _tokenService.GenerateRefreshToken();

        var refreshToken = new RefreshToken
        {
            Token = refreshTokenString,
            ExpiresAt = DateTime.UtcNow.AddDays(7), // Expirar em 7 dias
            UserId = user.Id
        };

        await _userRepository.SaveRefreshTokenAsync(refreshToken);

        return new TokenResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenString
        };
    }

    public async Task<bool> CreateUserAsync(SmartOrderManagement.Application.DTOs.User.CreateUserRequestDto request, Guid companyId)
    {
        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            // Email already in use
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
}
