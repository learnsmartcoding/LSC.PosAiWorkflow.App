using LSC.PosAiWorkflow.Api.Models.Approvals;
using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Abstractions.Services;
using LSC.PosAiWorkflow.Application.Approvals.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace LSC.PosAiWorkflow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ApprovalsController : ControllerBase
{
    private readonly IApprovalRequestQueryService _approvalRequestQueryService;
    private readonly IApprovalWorkflowService _approvalWorkflowService;

    public ApprovalsController(
        IApprovalRequestQueryService approvalRequestQueryService,
        IApprovalWorkflowService approvalWorkflowService)
    {
        _approvalRequestQueryService = approvalRequestQueryService;
        _approvalWorkflowService = approvalWorkflowService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRecentAsync([FromQuery] int take = 50, CancellationToken cancellationToken = default)
    {
        take = take <= 0 ? 50 : Math.Min(take, 200);

        var result = await _approvalRequestQueryService.GetRecentAsync(take, cancellationToken);
        return Ok(result);
    }

    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        var result = await _approvalRequestQueryService.GetPendingAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("{approvalRequestId:long}/approve")]
    public async Task<IActionResult> ApproveAsync(
        long approvalRequestId,
        [FromBody] ApprovalActionRequest request,
        CancellationToken cancellationToken = default)
    {
        await _approvalWorkflowService.ApproveAsync(
            approvalRequestId,
            new ApprovalActionRequestDto
            {
                ReviewedBy = request.ReviewedBy,
                ReviewComments = request.ReviewComments
            },
            cancellationToken);

        return Ok(new
        {
            Message = "Approval processed successfully.",
            ApprovalRequestId = approvalRequestId,
            Status = "Approved"
        });
    }

    [HttpPost("{approvalRequestId:long}/reject")]
    public async Task<IActionResult> RejectAsync(
        long approvalRequestId,
        [FromBody] ApprovalActionRequest request,
        CancellationToken cancellationToken = default)
    {
        await _approvalWorkflowService.RejectAsync(
            approvalRequestId,
            new ApprovalActionRequestDto
            {
                ReviewedBy = request.ReviewedBy,
                ReviewComments = request.ReviewComments
            },
            cancellationToken);

        return Ok(new
        {
            Message = "Approval processed successfully.",
            ApprovalRequestId = approvalRequestId,
            Status = "Rejected"
        });
    }
}