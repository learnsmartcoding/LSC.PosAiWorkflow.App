using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.AiDecisions.Dtos;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class AiDecisionQueryService : IAiDecisionQueryService
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public AiDecisionQueryService(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<AiDecisionListItemDto>> GetRecentAsync(int take = 50, CancellationToken cancellationToken = default)
    {
        return await
            (from decision in _dbContext.AiDecisions
             join product in _dbContext.Products
                 on decision.ProductId equals product.ProductId
             orderby decision.AiDecisionId descending
             select new AiDecisionListItemDto
             {
                 AiDecisionId = decision.AiDecisionId,
                 ProductId = decision.ProductId,
                 ProductName = product.Name,
                 StoreCode = decision.StoreCode ?? string.Empty,
                 CorrelationId = decision.CorrelationId,
                 DecisionType = decision.DecisionType,
                 RecommendedAction = decision.RecommendedAction,
                 ConfidenceScore = decision.ConfidenceScore,
                 RiskLevel = decision.RiskLevel,
                 DecisionStatus = decision.DecisionStatus,
                 SuggestedQuantity = decision.SuggestedQuantity,
                 EstimatedTotalCost = decision.EstimatedTotalCost,
                 RequiresApproval = decision.RequiresApproval,
                 ScenarioName = decision.ScenarioName,
                 CreatedUtc = decision.CreatedUtc
             })
            .AsNoTracking()
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<AiDecisionDetailDto?> GetByIdAsync(long aiDecisionId, CancellationToken cancellationToken = default)
    {
        return await
            (from decision in _dbContext.AiDecisions
             join product in _dbContext.Products
                 on decision.ProductId equals product.ProductId
             where decision.AiDecisionId == aiDecisionId
             select new AiDecisionDetailDto
             {
                 AiDecisionId = decision.AiDecisionId,
                 ProductId = decision.ProductId,
                 ProductName = product.Name,
                 StoreCode = decision.StoreCode ?? string.Empty,
                 CorrelationId = decision.CorrelationId,
                 DecisionType = decision.DecisionType,
                 EntityType = decision.EntityType,
                 EntityId = decision.EntityId,
                 ModelName = decision.ModelName,
                 PromptVersion = decision.PromptVersion,
                 InputSummaryJson = decision.InputSummaryJson,
                 DecisionJson = decision.DecisionJson,
                 Reasoning = decision.Reasoning ?? string.Empty,
                 RecommendedAction = decision.RecommendedAction,
                 ConfidenceScore = decision.ConfidenceScore,
                 RiskLevel = decision.RiskLevel,
                 DecisionStatus = decision.DecisionStatus,
                 SuggestedQuantity = decision.SuggestedQuantity,
                 EstimatedUnitCost = decision.EstimatedUnitCost,
                 EstimatedTotalCost = decision.EstimatedTotalCost,
                 RequiresApproval = decision.RequiresApproval,
                 ScenarioName = decision.ScenarioName,
                 CreatedUtc = decision.CreatedUtc,
                 ApprovedUtc = decision.ApprovedUtc,
                 RejectedUtc = decision.RejectedUtc,
                 ExecutedUtc = decision.ExecutedUtc
             })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
}