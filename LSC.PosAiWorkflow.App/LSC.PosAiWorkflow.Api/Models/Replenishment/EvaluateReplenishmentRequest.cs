namespace LSC.PosAiWorkflow.Api.Models.Replenishment;

public sealed class EvaluateReplenishmentRequest
{
    public long ProductId { get; set; }
    public string StoreCode { get; set; } = string.Empty;
    public string? ScenarioName { get; set; }
}