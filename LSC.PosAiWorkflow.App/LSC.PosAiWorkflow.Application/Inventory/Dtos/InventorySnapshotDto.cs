namespace LSC.PosAiWorkflow.Application.Inventory.Dtos;

public sealed class InventorySnapshotDto
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string StoreCode { get; set; } = string.Empty;
    public int QuantityOnHand { get; set; }
    public int QuantityReserved { get; set; }
    public int QuantityAvailable { get; set; }
    public int ReorderThreshold { get; set; }
    public int PreferredReorderQuantity { get; set; }
    public int LeadTimeDays { get; set; }
    public decimal CostPrice { get; set; }
    public decimal UnitPrice { get; set; }
}