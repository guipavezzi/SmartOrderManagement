using Microsoft.AspNetCore.Mvc;
using SmartOrderManagement.Application.Dtos;

[ApiController]
[Route("api/[Controller]")]
public class MenuController : Controller
{
    private readonly IMenuService _service;

    public MenuController(IMenuService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _service.GetMenusAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateMenuRequest request)
    {
        return Created(string.Empty, await _service.CreateMenuAsync(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        return Ok(await _service.GetMenuByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateMenuRequest request)
    {
        return Ok(await _service.UpdateMenuAsync(request, id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        return Ok(await _service.DeleteMenuAsync(id));
    }
}
