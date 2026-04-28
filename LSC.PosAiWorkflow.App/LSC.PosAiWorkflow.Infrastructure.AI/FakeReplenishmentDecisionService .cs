using System.Text.Json;
using LSC.PosAiWorkflow.Application.Abstractions.AI;
using LSC.PosAiWorkflow.Application.Inventory.Dtos;
using LSC.PosAiWorkflow.Application.Replenishment.Dtos;

namespace LSC.PosAiWorkflow.Infrastructure.AI;

public sealed class FakeReplenishmentDecisionService : IReplenishmentDecisionService
{
    public Task<ReplenishmentDecisionResultDto> EvaluateAsync(
        InventorySnapshotDto inventory,
        SalesVelocityDto salesVelocity,
        string? scenarioName,
        CancellationToken cancellationToken = default)
    {
        var reorderNeeded = inventory.QuantityAvailable <= inventory.ReorderThreshold;

        var suggestedQuantity = reorderNeeded
            ? Math.Max(
                inventory.PreferredReorderQuantity,
                (int)Math.Ceiling(salesVelocity.AverageUnitsPerDay * Math.Max(inventory.LeadTimeDays, 1)))
            : 0;

        var estimatedUnitCost = inventory.CostPrice;
        var estimatedTotalCost = estimatedUnitCost * suggestedQuantity;

        var result = new ReplenishmentDecisionResultDto
        {
            ReorderNeeded = reorderNeeded,
            SuggestedQuantity = suggestedQuantity,
            Reasoning = reorderNeeded
                ? $"Available stock {inventory.QuantityAvailable} is at or below threshold {inventory.ReorderThreshold}. Average daily sales is {salesVelocity.AverageUnitsPerDay:F2}."
                : $"Available stock {inventory.QuantityAvailable} is above threshold {inventory.ReorderThreshold}.",
            RecommendedAction = reorderNeeded ? "CreatePurchaseOrderDraft" : "NoAction",
            ConfidenceScore = 82,
            RiskLevel = estimatedTotalCost >= 5000 ? "Medium" : "Low",
            EstimatedUnitCost = estimatedUnitCost,
            EstimatedTotalCost = estimatedTotalCost,
            RequiresApproval = estimatedTotalCost >= 5000,
            InputSummaryJson = JsonSerializer.Serialize(new
            {
                inventory.ProductId,
                inventory.ProductName,
                inventory.StoreCode,
                inventory.QuantityAvailable,
                inventory.ReorderThreshold,
                inventory.PreferredReorderQuantity,
                inventory.LeadTimeDays,
                salesVelocity.DaysConsidered,
                salesVelocity.TotalUnitsSold,
                salesVelocity.AverageUnitsPerDay,
                scenarioName
            }),
            DecisionJson = JsonSerializer.Serialize(new
            {
                reorderNeeded,
                suggestedQuantity,
                estimatedUnitCost,
                estimatedTotalCost,
                scenarioName
            })
        };

        return Task.FromResult(result);
    }
}