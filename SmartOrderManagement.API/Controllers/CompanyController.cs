using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.DTOs.Auth;
using SmartOrderManagement.Application.Interfaces;

namespace SmartOrderManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ICurrentUserService _currentUserService;

    public CompanyController(IUserService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    [HttpPost("register-saas")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterSaas([FromBody] RegisterRequestDto request)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var response = await _userService.RegisterAsync(request);
        if (response == null)
        {
            return BadRequest(new { Message = "Email already in use or registration failed." });
        }

        return Created("", response);
    }

}
