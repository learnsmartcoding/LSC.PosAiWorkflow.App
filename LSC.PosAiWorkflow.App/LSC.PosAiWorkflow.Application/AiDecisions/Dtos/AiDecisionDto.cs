namespace LSC.PosAiWorkflow.Application.AiDecisions.Dtos;

public sealed class AiDecisionDto
{
    public long AiDecisionId { get; set; }
    public long ProductId { get; set; }
    public Guid CorrelationId { get; set; }
    public string DecisionType { get; set; } = string.Empty;
    public string DecisionStatus { get; set; } = string.Empty;
    public string RecommendedAction { get; set; } = string.Empty;
    public decimal? ConfidenceScore { get; set; }
    public string? RiskLevel { get; set; }
    public DateTime CreatedUtc { get; set; }
}