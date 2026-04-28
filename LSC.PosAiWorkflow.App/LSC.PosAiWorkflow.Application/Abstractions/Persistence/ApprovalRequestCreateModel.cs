namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public sealed class ApprovalRequestCreateModel
{
    public long AiDecisionId { get; set; }
    public string ApprovalType { get; set; } = "RestockApproval";
    public string Status { get; set; } = "Pending";
    public string RequestedBy { get; set; } = "System";
    public string? ScenarioName { get; set; }
    public bool IsSynthetic { get; set; } = true;
}