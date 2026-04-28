namespace LSC.PosAiWorkflow.Application.Replenishment.Dtos;

public sealed class ReplenishmentDecisionRequestDto
{
    public long ProductId { get; set; }
    public string StoreCode { get; set; } = string.Empty;
    public string? ScenarioName { get; set; }
}