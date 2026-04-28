using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Repositories;

public sealed class PurchaseOrderDraftRepository : IPurchaseOrderDraftRepository
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public PurchaseOrderDraftRepository(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(PurchaseOrderDraftCreateModel model, CancellationToken cancellationToken = default)
    {
        var entity = new PurchaseOrderDraft
        {
            AiDecisionId = model.AiDecisionId,
            ProductId = model.ProductId,
            StoreCode = model.StoreCode,
            SuggestedQuantity = model.SuggestedQuantity,
            EstimatedUnitCost = model.EstimatedUnitCost,
            EstimatedTotalCost = model.EstimatedTotalCost,
            DraftStatus = model.DraftStatus,
            Notes = model.Notes,
            ScenarioName = model.ScenarioName,
            IsSynthetic = model.IsSynthetic
        };

        _dbContext.PurchaseOrderDrafts.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.PurchaseOrderDraftId;
    }
}