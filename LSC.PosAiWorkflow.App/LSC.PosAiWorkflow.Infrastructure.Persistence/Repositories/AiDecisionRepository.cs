using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Repositories;

public sealed class AiDecisionRepository : IAiDecisionRepository
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public AiDecisionRepository(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(AiDecisionCreateModel model, CancellationToken cancellationToken = default)
    {
        var entity = new AiDecision
        {
            DecisionType = model.DecisionType,
            EntityType = model.EntityType,
            EntityId = model.EntityId,
            ProductId = model.ProductId,
            StoreCode = model.StoreCode,
            CorrelationId = model.CorrelationId,
            ModelName = model.ModelName,
            PromptVersion = model.PromptVersion,
            InputSummaryJson = model.InputSummaryJson,
            DecisionJson = model.DecisionJson,
            RecommendedAction = model.RecommendedAction,
            ConfidenceScore = model.ConfidenceScore,
            RiskLevel = model.RiskLevel,
            DecisionStatus = model.DecisionStatus,
            SuggestedQuantity = model.SuggestedQuantity,
            EstimatedUnitCost = model.EstimatedUnitCost,
            EstimatedTotalCost = model.EstimatedTotalCost,
            RequiresApproval = model.RequiresApproval,
            Reasoning = model.Reasoning,
            ScenarioName = model.ScenarioName,
            IsSynthetic = model.IsSynthetic
        };

        _dbContext.AiDecisions.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.AiDecisionId;
    }
}