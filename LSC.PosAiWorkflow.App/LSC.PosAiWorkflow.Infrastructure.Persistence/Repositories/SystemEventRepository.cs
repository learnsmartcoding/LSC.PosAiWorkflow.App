using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Repositories;

public sealed class SystemEventRepository : ISystemEventRepository
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public SystemEventRepository(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<long> CreateAsync(SystemEventCreateModel model, CancellationToken cancellationToken = default)
    {
        var entity = new SystemEvent
        {
            CorrelationId = model.CorrelationId,
            EventType = model.EventType,
            EntityType = model.EntityType,
            EntityId = model.EntityId,
            EventDataJson = model.EventDataJson,
            ScenarioName = model.ScenarioName,
            IsSynthetic = model.IsSynthetic,
            CreatedBy = model.CreatedBy
        };

        _dbContext.SystemEvents.Add(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity.SystemEventId;
    }
}