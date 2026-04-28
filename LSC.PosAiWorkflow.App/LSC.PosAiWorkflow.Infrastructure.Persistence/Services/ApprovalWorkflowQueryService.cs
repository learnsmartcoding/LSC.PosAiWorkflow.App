using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Approvals.Dtos;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class ApprovalWorkflowQueryService : IApprovalWorkflowQueryService
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public ApprovalWorkflowQueryService(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PendingApprovalDetailDto?> GetPendingApprovalDetailAsync(long approvalRequestId, CancellationToken cancellationToken = default)
    {
        return await
            (from approval in _dbContext.ApprovalRequests
             join decision in _dbContext.AiDecisions
                 on approval.AiDecisionId equals decision.AiDecisionId
             join product in _dbContext.Products
                 on decision.ProductId equals product.ProductId
             where approval.ApprovalRequestId == approvalRequestId
                   && approval.Status == "Pending"
             select new PendingApprovalDetailDto
             {
                 ApprovalRequestId = approval.ApprovalRequestId,
                 AiDecisionId = approval.AiDecisionId,
                 ProductId = decision.ProductId,
                 ProductName = product.Name,
                 StoreCode = decision.StoreCode ?? string.Empty,
                 ApprovalType = approval.ApprovalType,
                 Status = approval.Status,
                 RequestedBy = approval.RequestedBy,
                 RequestedUtc = approval.RequestedUtc,
                 ScenarioName = approval.ScenarioName,
                 SuggestedQuantity = decision.SuggestedQuantity ?? 0,
                 EstimatedUnitCost = decision.EstimatedUnitCost ?? 0,
                 EstimatedTotalCost = decision.EstimatedTotalCost ?? 0,
                 DecisionReasoning = decision.Reasoning,
                 CorrelationId = decision.CorrelationId
             })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }

}