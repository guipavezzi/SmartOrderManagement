using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.Auth;
using SmartOrderManagement.Application.Interfaces;
using System.Security.Claims;

namespace SmartOrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public UserController(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _userService.RegisterAsync(request);
        if (response == null)
        {
            return BadRequest(new { Message = "Email already in use or registration failed." });
        }

        return Created("", response);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _userService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new { Message = "Invalid email or password." });
        }

        return Ok(response);
    }

    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _userService.RefreshTokenAsync(request);
        if (response == null)
        {
            return Unauthorized(new { Message = "Invalid or expired refresh token." });
        }

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult GetMyProfile()
    {
        return Ok(new
        {
            UserId = _currentUserService.UserId,
            CompanyId = _currentUserService.CompanyId,
            Role = _currentUserService.Role.ToString(),
            Email = User.FindFirst(ClaimTypes.Email)?.Value
        });
    }
}
