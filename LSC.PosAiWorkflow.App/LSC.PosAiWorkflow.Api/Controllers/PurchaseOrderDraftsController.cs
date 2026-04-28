using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LSC.PosAiWorkflow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class PurchaseOrderDraftsController : ControllerBase
{
    private readonly IPurchaseOrderDraftQueryService _purchaseOrderDraftQueryService;

    public PurchaseOrderDraftsController(IPurchaseOrderDraftQueryService purchaseOrderDraftQueryService)
    {
        _purchaseOrderDraftQueryService = purchaseOrderDraftQueryService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecentAsync([FromQuery] int take = 50, CancellationToken cancellationToken = default)
    {
        take = take <= 0 ? 50 : Math.Min(take, 200);

        var result = await _purchaseOrderDraftQueryService.GetRecentAsync(take, cancellationToken);
        return Ok(result);
    }
}