namespace LSC.PosAiWorkflow.Api.Models.Approvals;

public sealed class ApprovalActionRequest
{
    public string ReviewedBy { get; set; } = string.Empty;
    public string? ReviewComments { get; set; }
}