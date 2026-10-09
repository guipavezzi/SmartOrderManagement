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


    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _userService.LoginAsync(request);
        if (response == null)
        {
            return Unauthorized(new { Message = "E-mail ou senha invÃ¡lidos." });
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
            return Unauthorized(new { Message = "Token de atualizaÃ§Ã£o invÃ¡lido ou expirado." });
        }

        return Ok(response);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUser([FromBody] SmartOrderManagement.Application.DTOs.User.CreateUserRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var companyId = _currentUserService.CompanyId;

        if (_currentUserService.Role != SmartOrderManagement.Domain.Enums.UserRole.Admin)
        {
            return Forbid();
        }

        var success = await _userService.CreateUserAsync(request, companyId);
        if (!success)
        {
            return BadRequest(new { Message = "NÃ£o foi possÃ­vel criar o usuÃ¡rio." });
        }

        return Created("", new { Message = "UsuÃ¡rio criado com sucesso." });
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
