using LSC.PosAiWorkflow.Application.Approvals.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Services;

public interface IApprovalWorkflowService
{
    Task ApproveAsync(long approvalRequestId, ApprovalActionRequestDto request, CancellationToken cancellationToken = default);
    Task RejectAsync(long approvalRequestId, ApprovalActionRequestDto request, CancellationToken cancellationToken = default);
}