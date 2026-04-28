using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Approvals.Dtos;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class ApprovalRequestQueryService : IApprovalRequestQueryService
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public ApprovalRequestQueryService(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ApprovalRequestListItemDto>> GetRecentAsync(int take = 50, CancellationToken cancellationToken = default)
    {
        return await
            (from approval in _dbContext.ApprovalRequests
             join decision in _dbContext.AiDecisions
                 on approval.AiDecisionId equals decision.AiDecisionId
             join product in _dbContext.Products
                 on decision.ProductId equals product.ProductId
             orderby approval.ApprovalRequestId descending
             select new ApprovalRequestListItemDto
             {
                 ApprovalRequestId = approval.ApprovalRequestId,
                 AiDecisionId = approval.AiDecisionId,
                 ProductId = decision.ProductId,
                 ProductName = product.Name,
                 ApprovalType = approval.ApprovalType,
                 Status = approval.Status,
                 RequestedBy = approval.RequestedBy,
                 RequestedUtc = approval.RequestedUtc,
                 ScenarioName = approval.ScenarioName
             })
            .AsNoTracking()
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ApprovalRequestListItemDto>> GetPendingAsync(CancellationToken cancellationToken = default)
    {
        return await
            (from approval in _dbContext.ApprovalRequests
             join decision in _dbContext.AiDecisions
                 on approval.AiDecisionId equals decision.AiDecisionId
             join product in _dbContext.Products
                 on decision.ProductId equals product.ProductId
             where approval.Status == "Pending"
             orderby approval.RequestedUtc descending
             select new ApprovalRequestListItemDto
             {
                 ApprovalRequestId = approval.ApprovalRequestId,
                 AiDecisionId = approval.AiDecisionId,
                 ProductId = decision.ProductId,
                 ProductName = product.Name,
                 ApprovalType = approval.ApprovalType,
                 Status = approval.Status,
                 RequestedBy = approval.RequestedBy,
                 RequestedUtc = approval.RequestedUtc,
                 ScenarioName = approval.ScenarioName
             })
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}