using LSC.PosAiWorkflow.Application.Inventory.Dtos;
using LSC.PosAiWorkflow.Application.Replenishment.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.AI;

public interface IReplenishmentDecisionService
{
    Task<ReplenishmentDecisionResultDto> EvaluateAsync(
        InventorySnapshotDto inventory,
        SalesVelocityDto salesVelocity,
        string? scenarioName,
        CancellationToken cancellationToken = default);
}