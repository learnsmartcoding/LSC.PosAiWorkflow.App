using System.Text.Json;
using LSC.PosAiWorkflow.Application.Inventory.Dtos;

namespace LSC.PosAiWorkflow.Infrastructure.AI.Prompts;

internal static class ReplenishmentPromptBuilder
{
    public static string Build(
        InventorySnapshotDto inventory,
        SalesVelocityDto salesVelocity,
        string? scenarioName)
    {
        var input = new
        {
            inventory.ProductId,
            inventory.ProductName,
            inventory.Sku,
            inventory.StoreCode,
            inventory.QuantityOnHand,
            inventory.QuantityReserved,
            inventory.QuantityAvailable,
            inventory.ReorderThreshold,
            inventory.PreferredReorderQuantity,
            inventory.LeadTimeDays,
            inventory.CostPrice,
            inventory.UnitPrice,
            SalesVelocity = new
            {
                salesVelocity.DaysConsidered,
                salesVelocity.TotalUnitsSold,
                salesVelocity.AverageUnitsPerDay,
                salesVelocity.FromUtc,
                salesVelocity.ToUtc
            },
            ScenarioName = scenarioName
        };

        var inputJson = JsonSerializer.Serialize(input, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return $$"""
You are an inventory replenishment AI assistant.

Your job:
- Analyze the provided inventory and recent sales data.
- Decide whether reorder is needed.
- Suggest a reorder quantity.
- Explain the reasoning briefly.
- Decide whether the recommendation should require approval.

Rules:
- Reorder is usually needed when QuantityAvailable is less than or equal to ReorderThreshold.
- Consider average daily sales and lead time days.
- SuggestedQuantity should be a realistic positive integer if reorder is needed, otherwise 0.
- RecommendedAction must be either "CreatePurchaseOrderDraft" or "NoAction".
- RiskLevel should be one of: "Low", "Medium", "High".
- ConfidenceScore should be between 0 and 100.
- RequiresApproval should be true if the recommendation looks high cost, high risk, or uncertain.

Return ONLY valid JSON.
Do not include markdown.
Do not include explanation outside JSON.

Expected JSON schema:
{
  "reorderNeeded": true,
  "suggestedQuantity": 80,
  "reasoning": "Short explanation",
  "recommendedAction": "CreatePurchaseOrderDraft",
  "confidenceScore": 85,
  "riskLevel": "Low",
  "requiresApproval": false
}

Business input:
{{inputJson}}
""";
    }
}