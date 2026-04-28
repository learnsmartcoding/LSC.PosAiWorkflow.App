using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores the replenishment action proposed or created from an approved AI decision.
/// </summary>
[Table("PurchaseOrderDraft")]
[Index("DraftStatus", Name = "IX_PurchaseOrderDraft_DraftStatus")]
[Index("ProductId", "CreatedUtc", Name = "IX_PurchaseOrderDraft_ProductId_CreatedUtc")]
[Index("AiDecisionId", Name = "UQ_PurchaseOrderDraft_AiDecisionId", IsUnique = true)]
public partial class PurchaseOrderDraft
{
    /// <summary>
    /// Primary key for purchase order draft.
    /// </summary>
    [Key]
    public long PurchaseOrderDraftId { get; set; }

    /// <summary>
    /// Reference to the AI decision that led to this draft.
    /// </summary>
    public long AiDecisionId { get; set; }

    /// <summary>
    /// Reference to the product being reordered.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Store/location code the replenishment is intended for.
    /// </summary>
    [StringLength(30)]
    public string StoreCode { get; set; } = null!;

    /// <summary>
    /// Quantity recommended for replenishment.
    /// </summary>
    public int SuggestedQuantity { get; set; }

    /// <summary>
    /// Estimated cost per unit at reorder time.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal EstimatedUnitCost { get; set; }

    /// <summary>
    /// Estimated total reorder value.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal EstimatedTotalCost { get; set; }

    /// <summary>
    /// Current state of the purchase order draft.
    /// </summary>
    [StringLength(30)]
    public string DraftStatus { get; set; } = null!;

    /// <summary>
    /// Optional business or AI notes related to the draft.
    /// </summary>
    [StringLength(500)]
    public string? Notes { get; set; }

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
    /// UTC timestamp when the draft was created.
    /// </summary>
    [Precision(3)]
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// UTC timestamp when the draft was approved, if applicable.
    /// </summary>
    [Precision(3)]
    public DateTime? ApprovedUtc { get; set; }

    [ForeignKey("AiDecisionId")]
    [InverseProperty("PurchaseOrderDraft")]
    public virtual AiDecision AiDecision { get; set; } = null!;

    [ForeignKey("ProductId")]
    [InverseProperty("PurchaseOrderDrafts")]
    public virtual Product Product { get; set; } = null!;
}
