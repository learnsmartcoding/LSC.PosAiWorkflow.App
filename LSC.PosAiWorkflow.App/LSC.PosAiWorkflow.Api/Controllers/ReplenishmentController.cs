using LSC.PosAiWorkflow.Api.Models.Replenishment;
using LSC.PosAiWorkflow.Application.Replenishment.Dtos;
using LSC.PosAiWorkflow.Application.Replenishment.Services;
using Microsoft.AspNetCore.Mvc;

namespace LSC.PosAiWorkflow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ReplenishmentController : ControllerBase
{
    private readonly IReplenishmentWorkflowOrchestrator _replenishmentWorkflowOrchestrator;

    public ReplenishmentController(IReplenishmentWorkflowOrchestrator replenishmentWorkflowOrchestrator)
    {
        _replenishmentWorkflowOrchestrator = replenishmentWorkflowOrchestrator;
    }

    [HttpPost("evaluate")]
    public async Task<IActionResult> EvaluateAsync(
        [FromBody] EvaluateReplenishmentRequest request,
        CancellationToken cancellationToken)
    {
        if (request.ProductId <= 0)
        {
            return BadRequest("ProductId must be greater than 0.");
        }

        if (string.IsNullOrWhiteSpace(request.StoreCode))
        {
            return BadRequest("StoreCode is required.");
        }

        var aiDecisionId = await _replenishmentWorkflowOrchestrator.EvaluateAsync(
            new ReplenishmentDecisionRequestDto
            {
                ProductId = request.ProductId,
                StoreCode = request.StoreCode.Trim(),
                ScenarioName = request.ScenarioName
            },
            cancellationToken);

        return Ok(new
        {
            Message = "Replenishment workflow executed successfully.",
            AiDecisionId = aiDecisionId
        });
    }
}