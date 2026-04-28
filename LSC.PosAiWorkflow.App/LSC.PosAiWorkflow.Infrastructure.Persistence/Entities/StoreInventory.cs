using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores current stock position for each product by store, used by the AI replenishment workflow.
/// </summary>
[Table("StoreInventory")]
[Index("ProductId", Name = "IX_StoreInventory_ProductId")]
[Index("StoreCode", Name = "IX_StoreInventory_StoreCode")]
[Index("ProductId", "StoreCode", Name = "UQ_StoreInventory_ProductId_StoreCode", IsUnique = true)]
public partial class StoreInventory
{
    /// <summary>
    /// Primary key for store inventory row.
    /// </summary>
    [Key]
    public long StoreInventoryId { get; set; }

    /// <summary>
    /// Reference to the product in stock.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Business code for the store or location.
    /// </summary>
    [StringLength(30)]
    public string StoreCode { get; set; } = null!;

    /// <summary>
    /// Total physical units currently on hand.
    /// </summary>
    public int QuantityOnHand { get; set; }

    /// <summary>
    /// Units reserved and not freely available for sale.
    /// </summary>
    public int QuantityReserved { get; set; }

    /// <summary>
    /// UTC timestamp for the most recent stock update.
    /// </summary>
    [Precision(3)]
    public DateTime LastStockUpdatedUtc { get; set; }

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
    /// UTC timestamp when the row was created.
    /// </summary>
    [Precision(3)]
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// UTC timestamp when the row was last updated.
    /// </summary>
    [Precision(3)]
    public DateTime? UpdatedUtc { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("StoreInventories")]
    public virtual Product Product { get; set; } = null!;
}
