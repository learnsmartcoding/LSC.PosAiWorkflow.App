using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores AI-generated replenishment recommendations, including structured inputs, outputs, and execution state.
/// </summary>
[Table("AiDecision")]
[Index("CorrelationId", Name = "IX_AiDecision_CorrelationId")]
[Index("DecisionStatus", Name = "IX_AiDecision_DecisionStatus")]
[Index("ProductId", "CreatedUtc", Name = "IX_AiDecision_ProductId_CreatedUtc")]
public partial class AiDecision
{
    /// <summary>
    /// Primary key for AI decision.
    /// </summary>
    [Key]
    public long AiDecisionId { get; set; }

    /// <summary>
    /// Type of AI decision, currently ReplenishmentRecommendation.
    /// </summary>
    [StringLength(50)]
    public string DecisionType { get; set; } = null!;

    /// <summary>
    /// Type of business entity the decision is associated with.
    /// </summary>
    [StringLength(50)]
    public string EntityType { get; set; } = null!;

    /// <summary>
    /// Identifier of the business entity the decision targets.
    /// </summary>
    public long EntityId { get; set; }

    /// <summary>
    /// Reference to the product the replenishment decision is for.
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// Correlation identifier used to trace the entire workflow.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Name of the AI model used for the decision.
    /// </summary>
    [StringLength(100)]
    public string ModelName { get; set; } = null!;

    /// <summary>
    /// Prompt version used when generating the decision.
    /// </summary>
    [StringLength(30)]
    public string PromptVersion { get; set; } = null!;

    /// <summary>
    /// JSON summary of the business context provided to the AI.
    /// </summary>
    public string InputSummaryJson { get; set; } = null!;

    /// <summary>
    /// JSON payload of the structured AI decision.
    /// </summary>
    public string DecisionJson { get; set; } = null!;

    /// <summary>
    /// High-level recommended action such as CreatePurchaseOrderDraft.
    /// </summary>
    [StringLength(100)]
    public string RecommendedAction { get; set; } = null!;

    /// <summary>
    /// Optional confidence score from 0 to 100.
    /// </summary>
    [Column(TypeName = "decimal(5, 2)")]
    public decimal? ConfidenceScore { get; set; }

    /// <summary>
    /// Optional risk classification for the decision.
    /// </summary>
    [StringLength(20)]
    public string? RiskLevel { get; set; }

    /// <summary>
    /// Execution state of the AI decision.
    /// </summary>
    [StringLength(30)]
    public string DecisionStatus { get; set; } = null!;

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
    /// UTC timestamp when the decision was created.
    /// </summary>
    [Precision(3)]
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// UTC timestamp when the decision was approved.
    /// </summary>
    [Precision(3)]
    public DateTime? ApprovedUtc { get; set; }

    /// <summary>
    /// UTC timestamp when the decision was rejected.
    /// </summary>
    [Precision(3)]
    public DateTime? RejectedUtc { get; set; }

    /// <summary>
    /// UTC timestamp when the decision was executed.
    /// </summary>
    [Precision(3)]
    public DateTime? ExecutedUtc { get; set; }

    /// <summary>
    /// Store/location code associated with the AI replenishment decision.
    /// </summary>
    [StringLength(30)]
    public string? StoreCode { get; set; }

    /// <summary>
    /// Quantity suggested by the AI for replenishment.
    /// </summary>
    public int? SuggestedQuantity { get; set; }

    /// <summary>
    /// Estimated unit cost used for the AI decision.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? EstimatedUnitCost { get; set; }

    /// <summary>
    /// Estimated total reorder cost used for the AI decision.
    /// </summary>
    [Column(TypeName = "decimal(18, 2)")]
    public decimal? EstimatedTotalCost { get; set; }

    /// <summary>
    /// Indicates whether the AI decision requires human approval.
    /// </summary>
    public bool? RequiresApproval { get; set; }

    /// <summary>
    /// Human-readable reasoning returned by the AI for the decision.
    /// </summary>
    [StringLength(1000)]
    public string? Reasoning { get; set; }

    [InverseProperty("AiDecision")]
    public virtual ApprovalRequest? ApprovalRequest { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("AiDecisions")]
    public virtual Product Product { get; set; } = null!;

    [InverseProperty("AiDecision")]
    public virtual PurchaseOrderDraft? PurchaseOrderDraft { get; set; }
}
