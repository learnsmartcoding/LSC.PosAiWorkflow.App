using LSC.PosAiWorkflow.Application.Replenishment.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IPurchaseOrderDraftQueryService
{
    Task<IReadOnlyList<PurchaseOrderDraftListItemDto>> GetRecentAsync(int take = 50, CancellationToken cancellationToken = default);
}