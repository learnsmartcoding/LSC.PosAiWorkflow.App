namespace LSC.PosAiWorkflow.Infrastructure.AI.Models;

public sealed class ReplenishmentDecisionResponseModel
{
    public bool ReorderNeeded { get; set; }
    public int SuggestedQuantity { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public decimal? ConfidenceScore { get; set; }
    public string? RiskLevel { get; set; }
    public bool RequiresApproval { get; set; }
}