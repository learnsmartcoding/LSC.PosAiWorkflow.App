using LSC.PosAiWorkflow.Application.Approvals.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IApprovalWorkflowQueryService
{
    Task<PendingApprovalDetailDto?> GetPendingApprovalDetailAsync(long approvalRequestId, CancellationToken cancellationToken = default);
}