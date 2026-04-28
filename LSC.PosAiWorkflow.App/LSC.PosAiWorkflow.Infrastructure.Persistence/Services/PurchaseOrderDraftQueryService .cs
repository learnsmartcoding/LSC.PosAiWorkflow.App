using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Replenishment.Dtos;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class PurchaseOrderDraftQueryService : IPurchaseOrderDraftQueryService
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public PurchaseOrderDraftQueryService(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<PurchaseOrderDraftListItemDto>> GetRecentAsync(int take = 50, CancellationToken cancellationToken = default)
    {
        return await
            (from draft in _dbContext.PurchaseOrderDrafts
             join product in _dbContext.Products
                 on draft.ProductId equals product.ProductId
             orderby draft.PurchaseOrderDraftId descending
             select new PurchaseOrderDraftListItemDto
             {
                 PurchaseOrderDraftId = draft.PurchaseOrderDraftId,
                 AiDecisionId = draft.AiDecisionId,
                 ProductId = draft.ProductId,
                 ProductName = product.Name,
                 StoreCode = draft.StoreCode,
                 SuggestedQuantity = draft.SuggestedQuantity,
                 EstimatedUnitCost = draft.EstimatedUnitCost,
                 EstimatedTotalCost = draft.EstimatedTotalCost,
                 DraftStatus = draft.DraftStatus,
                 Notes = draft.Notes,
                 ScenarioName = draft.ScenarioName,
                 CreatedUtc = draft.CreatedUtc
             })
            .AsNoTracking()
            .Take(take)
            .ToListAsync(cancellationToken);
    }
}