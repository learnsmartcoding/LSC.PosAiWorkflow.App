namespace LSC.PosAiWorkflow.Application.Approvals.Dtos;

public sealed class ApprovalActionRequestDto
{
    public string ReviewedBy { get; set; } = string.Empty;
    public string? ReviewComments { get; set; }
}