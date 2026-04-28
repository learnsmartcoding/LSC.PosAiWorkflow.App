using LSC.PosAiWorkflow.Application.AiDecisions.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IAiDecisionQueryService
{
    Task<IReadOnlyList<AiDecisionListItemDto>> GetRecentAsync(int take = 50, CancellationToken cancellationToken = default);
    Task<AiDecisionDetailDto?> GetByIdAsync(long aiDecisionId, CancellationToken cancellationToken = default);
}