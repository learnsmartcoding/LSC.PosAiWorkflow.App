using LSC.PosAiWorkflow.Application.Inventory.Dtos;

namespace LSC.PosAiWorkflow.Application.Abstractions.Persistence;

public interface IInventoryReadService
{
    Task<InventorySnapshotDto?> GetInventorySnapshotAsync(long productId, string storeCode, CancellationToken cancellationToken = default);
    Task<SalesVelocityDto> GetSalesVelocityAsync(long productId, int days, CancellationToken cancellationToken = default);
}