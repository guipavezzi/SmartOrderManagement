using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using SmartOrderManagement.Application.Dtos;
using SmartOrderManagement.Domain.Enums;

[ApiController]
[Route("api/[Controller]")]
public class OrderController : Controller
{
    private readonly IOrderService _service;
    
    public OrderController(IOrderService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync([FromQuery] bool activeOnly = false, [FromQuery] bool completedOnly = false, [FromQuery] bool includeArchived = false)
    {
        return Ok(await _service.GetOrdersAsync(activeOnly, completedOnly, includeArchived));
    }

    [HttpPost("close-shift")]
    public async Task<IActionResult> CloseShiftAsync()
    {
        var result = await _service.CloseShiftAsync();
        return Ok(new { message = result });
    }

    [HttpGet("metrics")]
    public async Task<IActionResult> GetMetricsAsync()
    {
        return Ok(await _service.GetMetricsAsync());
    }

    [HttpGet("analytics")]
    public async Task<IActionResult> GetAnalyticsAsync()
    {
        return Ok(await _service.GetAnalyticsAsync());
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateOrderRequest request)
    {
        return Created(string.Empty, await _service.CreateOrderAsync(request));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        return Ok(await _service.GetOrderByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,[FromBody] UpdateOrderRequest request)
    {
        return Ok(await _service.UpdateOrderAsync(request, id));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        return Ok(await _service.CancelOrderAsync(id));
    }

   [HttpPatch("{id}/status")]
    public async Task<IActionResult> UpdateStatusAsync(Guid id, [FromBody] Status status)
    {   
        return Ok(await _service.UpdateOrderStatusAsync(id, status));
    }
}