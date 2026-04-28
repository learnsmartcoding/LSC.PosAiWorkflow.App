namespace LSC.PosAiWorkflow.Infrastructure.AI.Configuration;

public sealed class OllamaOptions
{
    public string Endpoint { get; set; } = "http://localhost:11434";
    public string ModelId { get; set; } = "llama3.1";
    public string ServiceId { get; set; } = "ollama";
}