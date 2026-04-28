using LSC.PosAiWorkflow.Application.AiDecisions.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IAiDecisionRepository
{
    Task<long> CreateAsync(AiDecisionCreateModel model, CancellationToken cancellationToken = default);
}