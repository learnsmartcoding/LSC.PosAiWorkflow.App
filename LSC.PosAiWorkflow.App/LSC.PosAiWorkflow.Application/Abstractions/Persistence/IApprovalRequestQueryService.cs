using LSC.PosAiWorkflow.Application.Approvals.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IApprovalRequestQueryService
{
    Task<IReadOnlyList<ApprovalRequestListItemDto>> GetRecentAsync(int take = 50, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<ApprovalRequestListItemDto>> GetPendingAsync(CancellationToken cancellationToken = default);
}