using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Tracks stock changes over time for traceability, debugging, and workflow demonstration.
/// </summary>
[Table("InventoryAuditLog")]
[Index("ProductId", "ChangedUtc", Name = "IX_InventoryAuditLog_ProductId_ChangedUtc")]
[Index("StoreCode", "ChangedUtc", Name = "IX_InventoryAuditLog_StoreCode_ChangedUtc")]
public partial class InventoryAuditLog
{
    /// <summary>
    /// Primary key for inventory audit entry.
    /// </summary>
    [Key]
    public long InventoryAuditLogId { get; set; }

    /// <summary>
    /// Reference to the affected product.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Store/location code where the stock change occurred.
    /// </summary>
    [StringLength(30)]
    public string StoreCode { get; set; } = null!;

    /// <summary>
    /// Type of inventory movement such as Sale, ManualAdjustment, Restock, or PurchaseOrderCreated.
    /// </summary>
    [StringLength(30)]
    public string ChangeType { get; set; } = null!;

    /// <summary>
    /// Quantity on hand before the stock change.
    /// </summary>
    public int QuantityBefore { get; set; }

    /// <summary>
    /// Amount of stock added or removed; negative values are allowed for reductions.
    /// </summary>
    public int QuantityChanged { get; set; }

    /// <summary>
    /// Quantity on hand after the stock change.
    /// </summary>
    public int QuantityAfter { get; set; }

    /// <summary>
    /// Optional reference entity type such as Order or PurchaseOrderDraft.
    /// </summary>
    [StringLength(30)]
    public string? ReferenceType { get; set; }

    /// <summary>
    /// Optional reference entity identifier.
    /// </summary>
    public long? ReferenceId { get; set; }

    /// <summary>
    /// Optional reason for the stock adjustment.
    /// </summary>
    [StringLength(250)]
    public string? Reason { get; set; }

    /// <summary>
    /// Optional simulation scenario label.
    /// </summary>
    [StringLength(100)]
    public string? ScenarioName { get; set; }

    /// <summary>
    /// Indicates whether the row is synthetic/demo data.
    /// </summary>
    public bool IsSynthetic { get; set; }

    /// <summary>
    /// UTC timestamp when the stock change happened.
    /// </summary>
    [Precision(3)]
    public DateTime ChangedUtc { get; set; }

    /// <summary>
    /// User, system, or process that made the stock change.
    /// </summary>
    [StringLength(100)]
    public string ChangedBy { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("InventoryAuditLogs")]
    public virtual Product Product { get; set; } = null!;
}
