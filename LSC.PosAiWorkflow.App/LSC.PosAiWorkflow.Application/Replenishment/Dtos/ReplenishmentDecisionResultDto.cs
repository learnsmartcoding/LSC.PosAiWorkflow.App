namespace LSC.PosAiWorkflow.Application.Replenishment.Dtos;

public sealed class ReplenishmentDecisionResultDto
{
    public bool ReorderNeeded { get; set; }
    public int SuggestedQuantity { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public decimal? ConfidenceScore { get; set; }
    public string? RiskLevel { get; set; }
    public decimal EstimatedUnitCost { get; set; }
    public decimal EstimatedTotalCost { get; set; }
    public bool RequiresApproval { get; set; }
    public string InputSummaryJson { get; set; } = string.Empty;
    public string DecisionJson { get; set; } = string.Empty;
}