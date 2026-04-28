namespace LSC.PosAiWorkflow.Application.Approvals.Dtos;

public sealed class PendingApprovalDetailDto
{
    public long ApprovalRequestId { get; set; }
    public long AiDecisionId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty;
    public string ApprovalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime RequestedUtc { get; set; }
    public string? ScenarioName { get; set; }
    public int SuggestedQuantity { get; set; }
    public decimal EstimatedUnitCost { get; set; }
    public decimal EstimatedTotalCost { get; set; }
    public string? DecisionReasoning { get; set; }
    public Guid CorrelationId { get; set; }
}