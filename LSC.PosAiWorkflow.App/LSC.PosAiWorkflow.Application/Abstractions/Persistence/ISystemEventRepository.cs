namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface ISystemEventRepository
{
    Task<long> CreateAsync(SystemEventCreateModel model, CancellationToken cancellationToken = default);
}