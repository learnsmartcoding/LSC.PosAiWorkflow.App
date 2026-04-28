namespace LSC.PosAiWorkflow.Application.Replenishment.Dtos;

public sealed class PurchaseOrderDraftListItemDto
{
    public long PurchaseOrderDraftId { get; set; }
    public long AiDecisionId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty;
    public int SuggestedQuantity { get; set; }
    public decimal EstimatedUnitCost { get; set; }
    public decimal EstimatedTotalCost { get; set; }
    public string DraftStatus { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? ScenarioName { get; set; }
    public DateTime CreatedUtc { get; set; }
}