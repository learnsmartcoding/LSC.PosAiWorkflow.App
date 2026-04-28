using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores order headers representing completed or simulated sales transactions.
/// </summary>
[Index("OrderStatus", Name = "IX_Orders_OrderStatus")]
[Index("OrderUtc", Name = "IX_Orders_OrderUtc")]
[Index("OrderNumber", Name = "UQ_Orders_OrderNumber", IsUnique = true)]
public partial class Order
{
    /// <summary>
    /// Primary key for order header.
    /// </summary>
    [Key]
    public long OrderId { get; set; }

    /// <summary>
    /// Unique business-facing order number.
    /// </summary>
    [StringLength(40)]
    public string OrderNumber { get; set; } = null!;

    /// <summary>
    /// UTC timestamp when the order was placed.
    /// </summary>
    [Precision(3)]
    public DateTime OrderUtc { get; set; }

    /// <summary>
    /// Optional customer name for demo readability.
    /// </summary>
    [StringLength(150)]
    public string? CustomerName { get; set; }

    /// <summary>
    /// Current order status such as Pending, Completed, or Cancelled.
    /// </summary>
    [StringLength(30)]
    public string OrderStatus { get; set; } = null!;

    /// <summary>
    /// Total monetary value of the order.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalAmount { get; set; }

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

    [InverseProperty("Order")]
    public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
}
