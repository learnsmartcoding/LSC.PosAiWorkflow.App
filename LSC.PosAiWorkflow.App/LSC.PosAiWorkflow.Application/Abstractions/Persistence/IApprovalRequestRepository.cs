namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IApprovalRequestRepository
{
    Task<long> CreateAsync(ApprovalRequestCreateModel model, CancellationToken cancellationToken = default);
}