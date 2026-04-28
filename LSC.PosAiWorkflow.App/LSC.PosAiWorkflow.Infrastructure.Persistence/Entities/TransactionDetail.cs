using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores order line items used to calculate sales velocity and consumption trends for replenishment.
/// </summary>
[Index("OrderId", Name = "IX_TransactionDetails_OrderId")]
[Index("ProductId", Name = "IX_TransactionDetails_ProductId")]
[Index("ProductId", "CreatedUtc", Name = "IX_TransactionDetails_ProductId_CreatedUtc")]
public partial class TransactionDetail
{
    /// <summary>
    /// Primary key for the order line item.
    /// </summary>
    [Key]
    public long TransactionDetailId { get; set; }

    /// <summary>
    /// Reference to the parent order.
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// Reference to the product sold.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Quantity sold for this line item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Unit selling price used on the transaction.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Extended amount for the line item.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal LineTotal { get; set; }

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

    [ForeignKey("OrderId")]
    [InverseProperty("TransactionDetails")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("TransactionDetails")]
    public virtual Product Product { get; set; } = null!;
}
