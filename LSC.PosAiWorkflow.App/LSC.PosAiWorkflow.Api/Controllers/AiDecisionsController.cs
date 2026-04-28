using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LSC.PosAiWorkflow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AiDecisionsController : ControllerBase
{
    private readonly IAiDecisionQueryService _aiDecisionQueryService;

    public AiDecisionsController(IAiDecisionQueryService aiDecisionQueryService)
    {
        _aiDecisionQueryService = aiDecisionQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecentAsync([FromQuery] int take = 50, CancellationToken cancellationToken = default)
    {
        take = take <= 0 ? 50 : Math.Min(take, 200);

        var result = await _aiDecisionQueryService.GetRecentAsync(take, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{aiDecisionId:long}")]
    public async Task<IActionResult> GetByIdAsync(long aiDecisionId, CancellationToken cancellationToken = default)
    {
        var result = await _aiDecisionQueryService.GetByIdAsync(aiDecisionId, cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }
}