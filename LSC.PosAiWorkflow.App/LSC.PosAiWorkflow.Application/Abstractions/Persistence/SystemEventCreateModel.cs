namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public sealed class SystemEventCreateModel
{
    public Guid CorrelationId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public long? EntityId { get; set; }
    public string? EventDataJson { get; set; }
    public string? ScenarioName { get; set; }
    public bool IsSynthetic { get; set; } = true;
    public string CreatedBy { get; set; } = "System";
}