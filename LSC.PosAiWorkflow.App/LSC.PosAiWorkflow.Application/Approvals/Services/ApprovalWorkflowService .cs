using System.Text.Json;
using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Abstractions.Services;
using LSC.PosAiWorkflow.Application.Approvals.Dtos;

namespace LSC.PosAiWorkflow.Application.Approvals.Services;

public sealed class ApprovalWorkflowService : IApprovalWorkflowService
{
    private readonly IApprovalWorkflowQueryService _approvalWorkflowQueryService;
    private readonly IApprovalWorkflowRepository _approvalWorkflowRepository;
    private readonly IPurchaseOrderDraftRepository _purchaseOrderDraftRepository;
    private readonly ISystemEventRepository _systemEventRepository;

    public ApprovalWorkflowService(
        IApprovalWorkflowQueryService approvalWorkflowQueryService,
        IApprovalWorkflowRepository approvalWorkflowRepository,
        IPurchaseOrderDraftRepository purchaseOrderDraftRepository,
        ISystemEventRepository systemEventRepository)
    {
        _approvalWorkflowQueryService = approvalWorkflowQueryService;
        _approvalWorkflowRepository = approvalWorkflowRepository;
        _purchaseOrderDraftRepository = purchaseOrderDraftRepository;
        _systemEventRepository = systemEventRepository;
    }

    public async Task ApproveAsync(long approvalRequestId, ApprovalActionRequestDto request, CancellationToken cancellationToken = default)
    {
        var approval = await _approvalWorkflowQueryService.GetPendingApprovalDetailAsync(approvalRequestId, cancellationToken);

        if (approval is null)
        {
            throw new InvalidOperationException($"Pending approval not found for ApprovalRequestId={approvalRequestId}.");
        }

        var reviewedBy = string.IsNullOrWhiteSpace(request.ReviewedBy) ? "Reviewer" : request.ReviewedBy.Trim();

        await _approvalWorkflowRepository.UpdateApprovalRequestAsync(
            approvalRequestId,
            status: "Approved",
            reviewedBy: reviewedBy,
            reviewComments: request.ReviewComments,
            cancellationToken: cancellationToken);

        await _approvalWorkflowRepository.UpdateAiDecisionStatusAsync(
            approval.AiDecisionId,
            decisionStatus: "Approved",
            approvedUtc: DateTime.UtcNow,
            rejectedUtc: null,
            cancellationToken: cancellationToken);

        await _purchaseOrderDraftRepository.CreateAsync(new PurchaseOrderDraftCreateModel
        {
            AiDecisionId = approval.AiDecisionId,
            ProductId = approval.ProductId,
            StoreCode = approval.StoreCode,
            SuggestedQuantity = approval.SuggestedQuantity,
            EstimatedUnitCost = approval.EstimatedUnitCost,
            EstimatedTotalCost = approval.EstimatedTotalCost,
            DraftStatus = "Approved",
            Notes = approval.DecisionReasoning,
            ScenarioName = approval.ScenarioName,
            IsSynthetic = true
        }, cancellationToken);

        await _systemEventRepository.CreateAsync(new SystemEventCreateModel
        {
            CorrelationId = approval.CorrelationId,
            EventType = "ApprovalApproved",
            EntityType = "ApprovalRequest",
            EntityId = approval.ApprovalRequestId,
            EventDataJson = JsonSerializer.Serialize(new
            {
                approval.ApprovalRequestId,
                approval.AiDecisionId,
                reviewedBy,
                request.ReviewComments
            }),
            ScenarioName = approval.ScenarioName,
            IsSynthetic = true,
            CreatedBy = reviewedBy
        }, cancellationToken);

        await _systemEventRepository.CreateAsync(new SystemEventCreateModel
        {
            CorrelationId = approval.CorrelationId,
            EventType = "PurchaseOrderDraftCreatedAfterApproval",
            EntityType = "AiDecision",
            EntityId = approval.AiDecisionId,
            EventDataJson = JsonSerializer.Serialize(new
            {
                approval.ProductId,
                approval.StoreCode,
                approval.SuggestedQuantity,
                approval.EstimatedUnitCost,
                approval.EstimatedTotalCost
            }),
            ScenarioName = approval.ScenarioName,
            IsSynthetic = true,
            CreatedBy = reviewedBy
        }, cancellationToken);
    }

    public async Task RejectAsync(long approvalRequestId, ApprovalActionRequestDto request, CancellationToken cancellationToken = default)
    {
        var approval = await _approvalWorkflowQueryService.GetPendingApprovalDetailAsync(approvalRequestId, cancellationToken);

        if (approval is null)
        {
            throw new InvalidOperationException($"Pending approval not found for ApprovalRequestId={approvalRequestId}.");
        }

        var reviewedBy = string.IsNullOrWhiteSpace(request.ReviewedBy) ? "Reviewer" : request.ReviewedBy.Trim();

        await _approvalWorkflowRepository.UpdateApprovalRequestAsync(
            approvalRequestId,
            status: "Rejected",
            reviewedBy: reviewedBy,
            reviewComments: request.ReviewComments,
            cancellationToken: cancellationToken);

        await _approvalWorkflowRepository.UpdateAiDecisionStatusAsync(
            approval.AiDecisionId,
            decisionStatus: "Rejected",
            approvedUtc: null,
            rejectedUtc: DateTime.UtcNow,
            cancellationToken: cancellationToken);

        await _systemEventRepository.CreateAsync(new SystemEventCreateModel
        {
            CorrelationId = approval.CorrelationId,
            EventType = "ApprovalRejected",
            EntityType = "ApprovalRequest",
            EntityId = approval.ApprovalRequestId,
            EventDataJson = JsonSerializer.Serialize(new
            {
                approval.ApprovalRequestId,
                approval.AiDecisionId,
                reviewedBy,
                request.ReviewComments
            }),
            ScenarioName = approval.ScenarioName,
            IsSynthetic = true,
            CreatedBy = reviewedBy
        }, cancellationToken);
    }
}