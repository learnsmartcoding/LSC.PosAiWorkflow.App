using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores product master data used by sales, inventory, and AI replenishment decisioning.
/// </summary>
[Table("Product")]
[Index("CategoryId", Name = "IX_Product_CategoryId")]
[Index("Sku", Name = "UQ_Product_Sku", IsUnique = true)]
public partial class Product
{
    /// <summary>
    /// Primary key for product.
    /// </summary>
    [Key]
    public long ProductId { get; set; }

    /// <summary>
    /// Reference to the category the product belongs to.
    /// </summary>
    public long CategoryId { get; set; }

    /// <summary>
    /// Unique stock keeping unit.
    /// </summary>
    [StringLength(50)]
    public string Sku { get; set; } = null!;

    /// <summary>
    /// Human-readable product name.
    /// </summary>
    [StringLength(150)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Optional product description.
    /// </summary>
    [StringLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Selling price per unit.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Estimated cost price per unit used for replenishment value calculation.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal CostPrice { get; set; }

    /// <summary>
    /// Base stock threshold below which replenishment should be evaluated.
    /// </summary>
    public int ReorderThreshold { get; set; }

    /// <summary>
    /// Default reorder quantity suggestion before AI adjusts it.
    /// </summary>
    public int PreferredReorderQuantity { get; set; }

    /// <summary>
    /// Estimated supplier lead time in days.
    /// </summary>
    public int LeadTimeDays { get; set; }

    /// <summary>
    /// Indicates whether the product is active.
    /// </summary>
    public bool IsActive { get; set; }

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

    [InverseProperty("Product")]
    public virtual ICollection<AiDecision> AiDecisions { get; set; } = new List<AiDecision>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<InventoryAuditLog> InventoryAuditLogs { get; set; } = new List<InventoryAuditLog>();

    [InverseProperty("Product")]
    public virtual ICollection<PurchaseOrderDraft> PurchaseOrderDrafts { get; set; } = new List<PurchaseOrderDraft>();

    [InverseProperty("Product")]
    public virtual ICollection<StoreInventory> StoreInventories { get; set; } = new List<StoreInventory>();

    [InverseProperty("Product")]
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
