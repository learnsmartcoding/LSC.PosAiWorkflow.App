using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores human-in-the-loop approvals required for AI replenishment recommendations above configured thresholds.
/// </summary>
[Table("ApprovalRequest")]
[Index("RequestedUtc", Name = "IX_ApprovalRequest_RequestedUtc")]
[Index("Status", Name = "IX_ApprovalRequest_Status")]
[Index("AiDecisionId", Name = "UQ_ApprovalRequest_AiDecisionId", IsUnique = true)]
public partial class ApprovalRequest
{
    /// <summary>
    /// Primary key for approval request.
    /// </summary>
    [Key]
    public long ApprovalRequestId { get; set; }

    /// <summary>
    /// Reference to the AI decision being reviewed.
    /// </summary>
    public long AiDecisionId { get; set; }

    /// <summary>
    /// Type of approval request, currently RestockApproval.
    /// </summary>
    [StringLength(50)]
    public string ApprovalType { get; set; } = null!;

    /// <summary>
    /// Current review status of the approval request.
    /// </summary>
    [StringLength(30)]
    public string Status { get; set; } = null!;

    /// <summary>
    /// User or system that created the approval request.
    /// </summary>
    [StringLength(100)]
    public string RequestedBy { get; set; } = null!;

    /// <summary>
    /// UTC timestamp when approval was requested.
    /// </summary>
    [Precision(3)]
    public DateTime RequestedUtc { get; set; }

    /// <summary>
    /// User who reviewed the request.
    /// </summary>
    [StringLength(100)]
    public string? ReviewedBy { get; set; }

    /// <summary>
    /// UTC timestamp when the request was reviewed.
    /// </summary>
    [Precision(3)]
    public DateTime? ReviewedUtc { get; set; }

    /// <summary>
    /// Optional review comments from the approver.
    /// </summary>
    [StringLength(500)]
    public string? ReviewComments { get; set; }

    /// <summary>
    /// Optional simulation scenario label.
    /// </summary>
    [StringLength(100)]
    public string? ScenarioName { get; set; }

    /// <summary>
    /// Indicates whether the row is synthetic/demo data.
    /// </summary>
    public bool IsSynthetic { get; set; }

    [ForeignKey("AiDecisionId")]
    [InverseProperty("ApprovalRequest")]
    public virtual AiDecision AiDecision { get; set; } = null!;
}
