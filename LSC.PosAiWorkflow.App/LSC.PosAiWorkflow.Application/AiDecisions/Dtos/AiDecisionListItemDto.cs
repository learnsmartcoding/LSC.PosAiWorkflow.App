namespace LSC.PosAiWorkflow.Application.AiDecisions.Dtos;

public sealed class AiDecisionListItemDto
{
    public long AiDecisionId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty;
    public Guid CorrelationId { get; set; }
    public string DecisionType { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public decimal? ConfidenceScore { get; set; }
    public string? RiskLevel { get; set; }
    public string DecisionStatus { get; set; } = string.Empty;
    public int? SuggestedQuantity { get; set; }
    public decimal? EstimatedTotalCost { get; set; }
    public bool? RequiresApproval { get; set; }
    public string? ScenarioName { get; set; }
    public DateTime CreatedUtc { get; set; }
}