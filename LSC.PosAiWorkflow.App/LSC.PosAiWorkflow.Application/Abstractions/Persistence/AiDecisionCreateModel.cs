namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public sealed class AiDecisionCreateModel
{
    public string DecisionType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public long EntityId { get; set; }
    public long ProductId { get; set; }
    public string StoreCode { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string PromptVersion { get; set; } = string.Empty;
    public string InputSummaryJson { get; set; } = string.Empty;
    public string DecisionJson { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public decimal? ConfidenceScore { get; set; }
    public string? RiskLevel { get; set; }
    public string DecisionStatus { get; set; } = string.Empty;
    public int SuggestedQuantity { get; set; }
    public decimal EstimatedUnitCost { get; set; }
    public decimal EstimatedTotalCost { get; set; }
    public bool RequiresApproval { get; set; }
    public string Reasoning { get; set; } = string.Empty;
    public string? ScenarioName { get; set; }
    public bool IsSynthetic { get; set; } = true;
}