namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IApprovalWorkflowRepository
{
    Task UpdateApprovalRequestAsync(
        long approvalRequestId,
        string status,
        string reviewedBy,
        string? reviewComments,
        CancellationToken cancellationToken = default);

    Task UpdateAiDecisionStatusAsync(
        long aiDecisionId,
        string decisionStatus,
        DateTime? approvedUtc,
        DateTime? rejectedUtc,
        CancellationToken cancellationToken = default);
}