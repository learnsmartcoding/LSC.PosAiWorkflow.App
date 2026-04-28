using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LSC.PosAiWorkflow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SystemEventsController : ControllerBase
{
    private readonly ISystemEventQueryService _systemEventQueryService;

    public SystemEventsController(ISystemEventQueryService systemEventQueryService)
    {
        _systemEventQueryService = systemEventQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecentAsync([FromQuery] int take = 100, CancellationToken cancellationToken = default)
    {
        take = take <= 0 ? 100 : Math.Min(take, 500);

        var result = await _systemEventQueryService.GetRecentAsync(take, cancellationToken);
        return Ok(result);
    }

    [HttpGet("by-correlation/{correlationId:guid}")]
    public async Task<IActionResult> GetByCorrelationIdAsync(Guid correlationId, CancellationToken cancellationToken = default)
    {
        var result = await _systemEventQueryService.GetByCorrelationIdAsync(correlationId, cancellationToken);
        return Ok(result);
    }
}