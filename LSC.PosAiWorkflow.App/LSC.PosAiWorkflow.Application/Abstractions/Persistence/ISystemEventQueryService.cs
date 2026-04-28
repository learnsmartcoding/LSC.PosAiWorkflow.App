using LSC.PosAiWorkflow.Application.Common;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface ISystemEventQueryService
{
    Task<IReadOnlyList<SystemEventListItemDto>> GetRecentAsync(int take = 100, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<SystemEventListItemDto>> GetByCorrelationIdAsync(Guid correlationId, CancellationToken cancellationToken = default);
}