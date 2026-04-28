namespace LSC.PosAiWorkflow.Infrastructure.AI.Configuration;

public sealed class OpenAiOptions
{
    public string ModelId { get; set; } = "gpt-4o-mini";
    public string ApiKey { get; set; } = string.Empty;
    public string? OrgId { get; set; }
    public string ServiceId { get; set; } = "openai";
}