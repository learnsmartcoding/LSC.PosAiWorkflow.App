using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class ApprovalWorkflowRepository : IApprovalWorkflowRepository
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public ApprovalWorkflowRepository(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task UpdateApprovalRequestAsync(
        long approvalRequestId,
        string status,
        string reviewedBy,
        string? reviewComments,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.ApprovalRequests
            .FirstOrDefaultAsync(x => x.ApprovalRequestId == approvalRequestId, cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException($"ApprovalRequest not found for id={approvalRequestId}.");
        }

        entity.Status = status;
        entity.ReviewedBy = reviewedBy;
        entity.ReviewedUtc = DateTime.UtcNow;
        entity.ReviewComments = reviewComments;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAiDecisionStatusAsync(
        long aiDecisionId,
        string decisionStatus,
        DateTime? approvedUtc,
        DateTime? rejectedUtc,
        CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.AiDecisions
            .FirstOrDefaultAsync(x => x.AiDecisionId == aiDecisionId, cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException($"AiDecision not found for id={aiDecisionId}.");
        }

        entity.DecisionStatus = decisionStatus;
        entity.ApprovedUtc = approvedUtc;
        entity.RejectedUtc = rejectedUtc;

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}