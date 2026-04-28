using LSC.PosAiWorkflow.Application.Replenishment.Dtos;

namespace LSC.PosAiWorkflow.Application.Replenishment.Services;

public interface IReplenishmentWorkflowOrchestrator
{
    Task<long> EvaluateAsync(ReplenishmentDecisionRequestDto request, CancellationToken cancellationToken = default);
}