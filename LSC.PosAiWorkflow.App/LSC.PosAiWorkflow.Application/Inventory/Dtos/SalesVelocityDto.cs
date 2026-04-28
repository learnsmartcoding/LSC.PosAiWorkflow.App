namespace LSC.PosAiWorkflow.Application.Inventory.Dtos;

public sealed class SalesVelocityDto
{
    public long ProductId { get; set; }
    public int DaysConsidered { get; set; }
    public int TotalUnitsSold { get; set; }
    public decimal AverageUnitsPerDay { get; set; }
    public DateTime FromUtc { get; set; }
    public DateTime ToUtc { get; set; }
}