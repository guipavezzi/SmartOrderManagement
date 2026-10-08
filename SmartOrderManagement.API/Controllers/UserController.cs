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
            return Unauthorized(new { Message = "E-mail ou senha inválidos." });
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
            return Unauthorized(new { Message = "Token de atualização inválido ou expirado." });
        }

        return Ok(response);
    }

    [HttpPost]
    [Authorize] // Requires authentication to get the CompanyId from the JWT
    public async Task<IActionResult> CreateUser([FromBody] SmartOrderManagement.Application.DTOs.User.CreateUserRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // Get the CompanyId from the currently logged in user (the Admin creating the employee)
        var companyId = _currentUserService.CompanyId;

        // Optionally check if the current user is an Admin
        if (_currentUserService.Role != SmartOrderManagement.Domain.Enums.UserRole.Admin)
        {
            return Forbid();
        }

        var success = await _userService.CreateUserAsync(request, companyId);
        if (!success)
        {
            return BadRequest(new { Message = "Não foi possível criar o usuário." });
        }

        return Created("", new { Message = "Usuário criado com sucesso." });
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
