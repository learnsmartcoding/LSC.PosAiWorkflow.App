namespace LSC.PosAiWorkflow.Application.Approvals.Dtos;

public sealed class ApprovalRequestDto
{
    public long ApprovalRequestId { get; set; }
    public long AiDecisionId { get; set; }
    public string ApprovalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime RequestedUtc { get; set; }
}