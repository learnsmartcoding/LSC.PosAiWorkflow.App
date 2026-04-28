using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Inventory.Dtos;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Services;

public sealed class InventoryReadService : IInventoryReadService
{
    private readonly PosAiWorkflowSimDbContext _dbContext;

    public InventoryReadService(PosAiWorkflowSimDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<InventorySnapshotDto?> GetInventorySnapshotAsync(
        long productId,
        string storeCode,
        CancellationToken cancellationToken = default)
    {
        var result = await
            (from inventory in _dbContext.StoreInventories
             join product in _dbContext.Products
                 on inventory.ProductId equals product.ProductId
             where inventory.ProductId == productId
                   && inventory.StoreCode == storeCode
             select new InventorySnapshotDto
             {
                 ProductId = product.ProductId,
                 ProductName = product.Name,
                 Sku = product.Sku,
                 StoreCode = inventory.StoreCode,
                 QuantityOnHand = inventory.QuantityOnHand,
                 QuantityReserved = inventory.QuantityReserved,
                 QuantityAvailable = inventory.QuantityOnHand - inventory.QuantityReserved,
                 ReorderThreshold = product.ReorderThreshold,
                 PreferredReorderQuantity = product.PreferredReorderQuantity,
                 LeadTimeDays = product.LeadTimeDays,
                 CostPrice = product.CostPrice,
                 UnitPrice = product.UnitPrice
             })
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task<SalesVelocityDto> GetSalesVelocityAsync(
        long productId,
        int days,
        CancellationToken cancellationToken = default)
    {
        var toUtc = DateTime.UtcNow;
        var fromUtc = toUtc.AddDays(-days);

        var totalUnitsSold = await
            (from td in _dbContext.TransactionDetails
             join order in _dbContext.Orders
                 on td.OrderId equals order.OrderId
             where td.ProductId == productId
                   && order.OrderStatus == "Completed"
                   && order.OrderUtc >= fromUtc
                   && order.OrderUtc <= toUtc
             select td.Quantity)
            .SumAsync(cancellationToken);

        return new SalesVelocityDto
        {
            ProductId = productId,
            DaysConsidered = days,
            TotalUnitsSold = totalUnitsSold,
            AverageUnitsPerDay = days == 0 ? 0 : Math.Round((decimal)totalUnitsSold / days, 2),
            FromUtc = fromUtc,
            ToUtc = toUtc
        };
    }
}