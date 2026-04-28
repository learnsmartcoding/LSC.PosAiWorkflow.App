namespace LSC.PosAiWorkflow.Application.Common;

public sealed class SystemEventListItemDto
{
    public long SystemEventId { get; set; }
    public Guid CorrelationId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public long? EntityId { get; set; }
    public string? EventDataJson { get; set; }
    public string? ScenarioName { get; set; }
    public DateTime CreatedUtc { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
}