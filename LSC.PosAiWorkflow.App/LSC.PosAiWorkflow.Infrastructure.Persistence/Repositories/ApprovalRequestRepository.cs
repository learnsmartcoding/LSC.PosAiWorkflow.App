using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Repositories;

public sealed class ApprovalRequestRepository : IApprovalRequestRepository
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public ApprovalRequestRepository(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(ApprovalRequestCreateModel model, CancellationToken cancellationToken = default)
    {
        var entity = new ApprovalRequest
        {
            AiDecisionId = model.AiDecisionId,
            ApprovalType = model.ApprovalType,
            Status = model.Status,
            RequestedBy = model.RequestedBy,
            ScenarioName = model.ScenarioName,
            IsSynthetic = model.IsSynthetic
        };

        _dbContext.ApprovalRequests.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.ApprovalRequestId;
    }
}