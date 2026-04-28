namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public sealed class PurchaseOrderDraftCreateModel
{
    public long AiDecisionId { get; set; }
    public long ProductId { get; set; }
    public string StoreCode { get; set; } = string.Empty;
    public int SuggestedQuantity { get; set; }
    public decimal EstimatedUnitCost { get; set; }
    public decimal EstimatedTotalCost { get; set; }
    public string DraftStatus { get; set; } = "Draft";
    public string? Notes { get; set; }
    public string? ScenarioName { get; set; }
    public bool IsSynthetic { get; set; } = true;
}