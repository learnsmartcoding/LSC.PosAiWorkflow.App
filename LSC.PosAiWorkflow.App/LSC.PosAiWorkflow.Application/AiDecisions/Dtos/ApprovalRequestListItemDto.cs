namespace LSC.PosAiWorkflow.Application.Approvals.Dtos;

public sealed class ApprovalRequestListItemDto
{
    public long ApprovalRequestId { get; set; }
    public long AiDecisionId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ApprovalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime RequestedUtc { get; set; }
    public string? ScenarioName { get; set; }
}