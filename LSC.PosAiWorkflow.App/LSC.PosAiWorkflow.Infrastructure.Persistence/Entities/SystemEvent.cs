using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores workflow-level events for full traceability across orders, AI decisions, approvals, and execution steps.
/// </summary>
[Table("SystemEvent")]
[Index("CorrelationId", "CreatedUtc", Name = "IX_SystemEvent_CorrelationId_CreatedUtc")]
[Index("EventType", "CreatedUtc", Name = "IX_SystemEvent_EventType_CreatedUtc")]
public partial class SystemEvent
{
    /// <summary>
    /// Primary key for workflow event.
    /// </summary>
    [Key]
    public long SystemEventId { get; set; }

    /// <summary>
    /// Correlation identifier tying together one workflow execution path.
    /// </summary>
    public Guid CorrelationId { get; set; }

    /// <summary>
    /// Type of event such as OrderPlaced, InventoryReduced, AiDecisionCreated, or ApprovalRequested.
    /// </summary>
    [StringLength(100)]
    public string EventType { get; set; } = null!;

    /// <summary>
    /// Type of business entity related to the event.
    /// </summary>
    [StringLength(50)]
    public string EntityType { get; set; } = null!;

    /// <summary>
    /// Optional identifier of the related entity.
    /// </summary>
    public long? EntityId { get; set; }

    /// <summary>
    /// Optional JSON payload describing the event details.
    /// </summary>
    public string? EventDataJson { get; set; }

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
    /// UTC timestamp when the event was recorded.
    /// </summary>
    [Precision(3)]
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// User, process, or service that recorded the event.
    /// </summary>
    [StringLength(100)]
    public string CreatedBy { get; set; } = null!;
}
