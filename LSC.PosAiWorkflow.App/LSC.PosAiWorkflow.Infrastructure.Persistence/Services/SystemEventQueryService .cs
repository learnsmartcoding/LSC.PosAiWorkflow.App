using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Common;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class SystemEventQueryService : ISystemEventQueryService
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public SystemEventQueryService(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<SystemEventListItemDto>> GetRecentAsync(int take = 100, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SystemEvents
            .AsNoTracking()
            .OrderByDescending(x => x.SystemEventId)
            .Take(take)
            .Select(x => new SystemEventListItemDto
            {
                SystemEventId = x.SystemEventId,
                CorrelationId = x.CorrelationId,
                EventType = x.EventType,
                EntityType = x.EntityType,
                EntityId = x.EntityId,
                EventDataJson = x.EventDataJson,
                ScenarioName = x.ScenarioName,
                CreatedUtc = x.CreatedUtc,
                CreatedBy = x.CreatedBy
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemEventListItemDto>> GetByCorrelationIdAsync(Guid correlationId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.SystemEvents
            .AsNoTracking()
            .Where(x => x.CorrelationId == correlationId)
            .OrderBy(x => x.SystemEventId)
            .Select(x => new SystemEventListItemDto
            {
                SystemEventId = x.SystemEventId,
                CorrelationId = x.CorrelationId,
                EventType = x.EventType,
                EntityType = x.EntityType,
                EntityId = x.EntityId,
                EventDataJson = x.EventDataJson,
                ScenarioName = x.ScenarioName,
                CreatedUtc = x.CreatedUtc,
                CreatedBy = x.CreatedBy
            })
            .ToListAsync(cancellationToken);
    }
}