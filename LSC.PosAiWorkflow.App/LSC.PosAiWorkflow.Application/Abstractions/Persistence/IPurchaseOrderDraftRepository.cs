namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IPurchaseOrderDraftRepository
{
    Task<long> CreateAsync(PurchaseOrderDraftCreateModel model, CancellationToken cancellationToken = default);
}