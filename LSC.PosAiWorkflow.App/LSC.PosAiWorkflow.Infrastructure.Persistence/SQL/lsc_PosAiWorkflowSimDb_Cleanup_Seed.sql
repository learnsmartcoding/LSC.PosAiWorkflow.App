
-- cleanup script

SET NOCOUNT ON;
GO

BEGIN TRY
    BEGIN TRANSACTION;

    DELETE FROM dbo.PurchaseOrderDraft;
    DELETE FROM dbo.ApprovalRequest;
    DELETE FROM dbo.SystemEvent;
    DELETE FROM dbo.AiDecision;
    DELETE FROM dbo.InventoryAuditLog;
    DELETE FROM dbo.TransactionDetails;
    DELETE FROM dbo.Orders;
    DELETE FROM dbo.StoreInventory;
    DELETE FROM dbo.Product;
    DELETE FROM dbo.Category;

    COMMIT TRANSACTION;
    PRINT 'Demo data cleanup completed successfully.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;

    THROW;
END CATCH;
GO





SELECT
    ProductId,
    Sku,
    Name,
    CostPrice,
    ReorderThreshold,
    PreferredReorderQuantity,
    LeadTimeDays,
    ScenarioName
FROM dbo.Product
ORDER BY ProductId;

SELECT
    si.StoreInventoryId,
    p.Sku,
    p.Name,
    si.StoreCode,
    si.QuantityOnHand,
    si.QuantityReserved,
    (si.QuantityOnHand - si.QuantityReserved) AS QuantityAvailable,
    p.ReorderThreshold,
    si.ScenarioName
FROM dbo.StoreInventory si
INNER JOIN dbo.Product p
    ON si.ProductId = p.ProductId
ORDER BY si.StoreInventoryId;


SELECT
    p.ProductId,
    p.Sku,
    p.Name,
    SUM(td.Quantity) AS TotalUnitsSoldLast7Days
FROM dbo.TransactionDetails td
INNER JOIN dbo.Orders o
    ON td.OrderId = o.OrderId
INNER JOIN dbo.Product p
    ON td.ProductId = p.ProductId
WHERE o.OrderStatus = 'Completed'
  AND o.OrderUtc >= DATEADD(DAY, -7, SYSUTCDATETIME())
GROUP BY p.ProductId, p.Sku, p.Name
ORDER BY p.ProductId;



SELECT ApprovalRequestId, AiDecisionId, Status, ReviewedBy, ReviewedUtc, ReviewComments, ScenarioName
FROM dbo.ApprovalRequest
ORDER BY ApprovalRequestId DESC;

SELECT AiDecisionId, DecisionStatus, ApprovedUtc, RejectedUtc, ScenarioName
FROM dbo.AiDecision
ORDER BY AiDecisionId DESC;

SELECT PurchaseOrderDraftId, AiDecisionId, DraftStatus, ScenarioName, EstimatedTotalCost
FROM dbo.PurchaseOrderDraft
ORDER BY PurchaseOrderDraftId DESC;