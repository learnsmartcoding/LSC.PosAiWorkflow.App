using System;
using System.Collections.Generic;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Context;

public partial class PosAiWorkflowSimDbContext : DbContext
{
    public PosAiWorkflowSimDbContext()
    {
    }

    public PosAiWorkflowSimDbContext(DbContextOptions<PosAiWorkflowSimDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AiDecision> AiDecisions { get; set; }

    public virtual DbSet<ApprovalRequest> ApprovalRequests { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<InventoryAuditLog> InventoryAuditLogs { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<PurchaseOrderDraft> PurchaseOrderDrafts { get; set; }

    public virtual DbSet<StoreInventory> StoreInventories { get; set; }

    public virtual DbSet<SystemEvent> SystemEvents { get; set; }

    public virtual DbSet<TransactionDetail> TransactionDetails { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AiDecision>(entity =>
        {
            entity.ToTable("AiDecision", tb => tb.HasComment("Stores AI-generated replenishment recommendations, including structured inputs, outputs, and execution state."));

            entity.Property(e => e.AiDecisionId).HasComment("Primary key for AI decision.");
            entity.Property(e => e.ApprovedUtc).HasComment("UTC timestamp when the decision was approved.");
            entity.Property(e => e.ConfidenceScore).HasComment("Optional confidence score from 0 to 100.");
            entity.Property(e => e.CorrelationId).HasComment("Correlation identifier used to trace the entire workflow.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the decision was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_AiDecision_CreatedUtc");
            entity.Property(e => e.DecisionJson).HasComment("JSON payload of the structured AI decision.");
            entity.Property(e => e.DecisionStatus).HasComment("Execution state of the AI decision.");
            entity.Property(e => e.DecisionType).HasComment("Type of AI decision, currently ReplenishmentRecommendation.");
            entity.Property(e => e.EntityId).HasComment("Identifier of the business entity the decision targets.");
            entity.Property(e => e.EntityType).HasComment("Type of business entity the decision is associated with.");
            entity.Property(e => e.EstimatedTotalCost).HasComment("Estimated total reorder cost used for the AI decision.");
            entity.Property(e => e.EstimatedUnitCost).HasComment("Estimated unit cost used for the AI decision.");
            entity.Property(e => e.ExecutedUtc).HasComment("UTC timestamp when the decision was executed.");
            entity.Property(e => e.InputSummaryJson).HasComment("JSON summary of the business context provided to the AI.");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_AiDecision_IsSynthetic");
            entity.Property(e => e.ModelName).HasComment("Name of the AI model used for the decision.");
            entity.Property(e => e.ProductId).HasComment("Reference to the product the replenishment decision is for.");
            entity.Property(e => e.PromptVersion).HasComment("Prompt version used when generating the decision.");
            entity.Property(e => e.Reasoning).HasComment("Human-readable reasoning returned by the AI for the decision.");
            entity.Property(e => e.RecommendedAction).HasComment("High-level recommended action such as CreatePurchaseOrderDraft.");
            entity.Property(e => e.RejectedUtc).HasComment("UTC timestamp when the decision was rejected.");
            entity.Property(e => e.RequiresApproval).HasComment("Indicates whether the AI decision requires human approval.");
            entity.Property(e => e.RiskLevel).HasComment("Optional risk classification for the decision.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.StoreCode).HasComment("Store/location code associated with the AI replenishment decision.");
            entity.Property(e => e.SuggestedQuantity).HasComment("Quantity suggested by the AI for replenishment.");

            entity.HasOne(d => d.Product).WithMany(p => p.AiDecisions).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ApprovalRequest>(entity =>
        {
            entity.ToTable("ApprovalRequest", tb => tb.HasComment("Stores human-in-the-loop approvals required for AI replenishment recommendations above configured thresholds."));

            entity.Property(e => e.ApprovalRequestId).HasComment("Primary key for approval request.");
            entity.Property(e => e.AiDecisionId).HasComment("Reference to the AI decision being reviewed.");
            entity.Property(e => e.ApprovalType).HasComment("Type of approval request, currently RestockApproval.");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_ApprovalRequest_IsSynthetic");
            entity.Property(e => e.RequestedBy).HasComment("User or system that created the approval request.");
            entity.Property(e => e.RequestedUtc)
                .HasComment("UTC timestamp when approval was requested.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_ApprovalRequest_RequestedUtc");
            entity.Property(e => e.ReviewComments).HasComment("Optional review comments from the approver.");
            entity.Property(e => e.ReviewedBy).HasComment("User who reviewed the request.");
            entity.Property(e => e.ReviewedUtc).HasComment("UTC timestamp when the request was reviewed.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.Status).HasComment("Current review status of the approval request.");

            entity.HasOne(d => d.AiDecision).WithOne(p => p.ApprovalRequest).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category", tb => tb.HasComment("Stores product categories used to group products for inventory and replenishment analysis."));

            entity.Property(e => e.CategoryId).HasComment("Primary key for category.");
            entity.Property(e => e.Code).HasComment("Short business code for the category.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the row was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Category_CreatedUtc");
            entity.Property(e => e.Description).HasComment("Optional category description.");
            entity.Property(e => e.IsActive)
                .HasComment("Indicates whether the category is active.")
                .HasDefaultValue(true, "DF_Category_IsActive");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row was created from synthetic/demo data.")
                .HasDefaultValue(true, "DF_Category_IsSynthetic");
            entity.Property(e => e.Name).HasComment("Human-readable category name.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label such as HolidaySpike or NormalSales.");
            entity.Property(e => e.UpdatedUtc).HasComment("UTC timestamp when the row was last updated.");
        });

        modelBuilder.Entity<InventoryAuditLog>(entity =>
        {
            entity.ToTable("InventoryAuditLog", tb => tb.HasComment("Tracks stock changes over time for traceability, debugging, and workflow demonstration."));

            entity.Property(e => e.InventoryAuditLogId).HasComment("Primary key for inventory audit entry.");
            entity.Property(e => e.ChangeType).HasComment("Type of inventory movement such as Sale, ManualAdjustment, Restock, or PurchaseOrderCreated.");
            entity.Property(e => e.ChangedBy).HasComment("User, system, or process that made the stock change.");
            entity.Property(e => e.ChangedUtc)
                .HasComment("UTC timestamp when the stock change happened.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_InventoryAuditLog_ChangedUtc");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_InventoryAuditLog_IsSynthetic");
            entity.Property(e => e.ProductId).HasComment("Reference to the affected product.");
            entity.Property(e => e.QuantityAfter).HasComment("Quantity on hand after the stock change.");
            entity.Property(e => e.QuantityBefore).HasComment("Quantity on hand before the stock change.");
            entity.Property(e => e.QuantityChanged).HasComment("Amount of stock added or removed; negative values are allowed for reductions.");
            entity.Property(e => e.Reason).HasComment("Optional reason for the stock adjustment.");
            entity.Property(e => e.ReferenceId).HasComment("Optional reference entity identifier.");
            entity.Property(e => e.ReferenceType).HasComment("Optional reference entity type such as Order or PurchaseOrderDraft.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.StoreCode).HasComment("Store/location code where the stock change occurred.");

            entity.HasOne(d => d.Product).WithMany(p => p.InventoryAuditLogs).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Stores order headers representing completed or simulated sales transactions."));

            entity.Property(e => e.OrderId).HasComment("Primary key for order header.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the row was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Orders_CreatedUtc");
            entity.Property(e => e.CustomerName).HasComment("Optional customer name for demo readability.");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_Orders_IsSynthetic");
            entity.Property(e => e.OrderNumber).HasComment("Unique business-facing order number.");
            entity.Property(e => e.OrderStatus).HasComment("Current order status such as Pending, Completed, or Cancelled.");
            entity.Property(e => e.OrderUtc).HasComment("UTC timestamp when the order was placed.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.TotalAmount).HasComment("Total monetary value of the order.");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product", tb => tb.HasComment("Stores product master data used by sales, inventory, and AI replenishment decisioning."));

            entity.Property(e => e.ProductId).HasComment("Primary key for product.");
            entity.Property(e => e.CategoryId).HasComment("Reference to the category the product belongs to.");
            entity.Property(e => e.CostPrice).HasComment("Estimated cost price per unit used for replenishment value calculation.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the row was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_Product_CreatedUtc");
            entity.Property(e => e.Description).HasComment("Optional product description.");
            entity.Property(e => e.IsActive)
                .HasComment("Indicates whether the product is active.")
                .HasDefaultValue(true, "DF_Product_IsActive");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_Product_IsSynthetic");
            entity.Property(e => e.LeadTimeDays).HasComment("Estimated supplier lead time in days.");
            entity.Property(e => e.Name).HasComment("Human-readable product name.");
            entity.Property(e => e.PreferredReorderQuantity).HasComment("Default reorder quantity suggestion before AI adjusts it.");
            entity.Property(e => e.ReorderThreshold).HasComment("Base stock threshold below which replenishment should be evaluated.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.Sku).HasComment("Unique stock keeping unit.");
            entity.Property(e => e.UnitPrice).HasComment("Selling price per unit.");
            entity.Property(e => e.UpdatedUtc).HasComment("UTC timestamp when the row was last updated.");

            entity.HasOne(d => d.Category).WithMany(p => p.Products).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<PurchaseOrderDraft>(entity =>
        {
            entity.ToTable("PurchaseOrderDraft", tb => tb.HasComment("Stores the replenishment action proposed or created from an approved AI decision."));

            entity.Property(e => e.PurchaseOrderDraftId).HasComment("Primary key for purchase order draft.");
            entity.Property(e => e.AiDecisionId).HasComment("Reference to the AI decision that led to this draft.");
            entity.Property(e => e.ApprovedUtc).HasComment("UTC timestamp when the draft was approved, if applicable.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the draft was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_PurchaseOrderDraft_CreatedUtc");
            entity.Property(e => e.DraftStatus).HasComment("Current state of the purchase order draft.");
            entity.Property(e => e.EstimatedTotalCost).HasComment("Estimated total reorder value.");
            entity.Property(e => e.EstimatedUnitCost).HasComment("Estimated cost per unit at reorder time.");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_PurchaseOrderDraft_IsSynthetic");
            entity.Property(e => e.Notes).HasComment("Optional business or AI notes related to the draft.");
            entity.Property(e => e.ProductId).HasComment("Reference to the product being reordered.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.StoreCode).HasComment("Store/location code the replenishment is intended for.");
            entity.Property(e => e.SuggestedQuantity).HasComment("Quantity recommended for replenishment.");

            entity.HasOne(d => d.AiDecision).WithOne(p => p.PurchaseOrderDraft).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderDrafts).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<StoreInventory>(entity =>
        {
            entity.ToTable("StoreInventory", tb => tb.HasComment("Stores current stock position for each product by store, used by the AI replenishment workflow."));

            entity.Property(e => e.StoreInventoryId).HasComment("Primary key for store inventory row.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the row was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_StoreInventory_CreatedUtc");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_StoreInventory_IsSynthetic");
            entity.Property(e => e.LastStockUpdatedUtc)
                .HasComment("UTC timestamp for the most recent stock update.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_StoreInventory_LastStockUpdatedUtc");
            entity.Property(e => e.ProductId).HasComment("Reference to the product in stock.");
            entity.Property(e => e.QuantityOnHand).HasComment("Total physical units currently on hand.");
            entity.Property(e => e.QuantityReserved).HasComment("Units reserved and not freely available for sale.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.StoreCode).HasComment("Business code for the store or location.");
            entity.Property(e => e.UpdatedUtc).HasComment("UTC timestamp when the row was last updated.");

            entity.HasOne(d => d.Product).WithMany(p => p.StoreInventories).OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<SystemEvent>(entity =>
        {
            entity.ToTable("SystemEvent", tb => tb.HasComment("Stores workflow-level events for full traceability across orders, AI decisions, approvals, and execution steps."));

            entity.Property(e => e.SystemEventId).HasComment("Primary key for workflow event.");
            entity.Property(e => e.CorrelationId).HasComment("Correlation identifier tying together one workflow execution path.");
            entity.Property(e => e.CreatedBy).HasComment("User, process, or service that recorded the event.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the event was recorded.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_SystemEvent_CreatedUtc");
            entity.Property(e => e.EntityId).HasComment("Optional identifier of the related entity.");
            entity.Property(e => e.EntityType).HasComment("Type of business entity related to the event.");
            entity.Property(e => e.EventDataJson).HasComment("Optional JSON payload describing the event details.");
            entity.Property(e => e.EventType).HasComment("Type of event such as OrderPlaced, InventoryReduced, AiDecisionCreated, or ApprovalRequested.");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_SystemEvent_IsSynthetic");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
        });

        modelBuilder.Entity<TransactionDetail>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Stores order line items used to calculate sales velocity and consumption trends for replenishment."));

            entity.Property(e => e.TransactionDetailId).HasComment("Primary key for the order line item.");
            entity.Property(e => e.CreatedUtc)
                .HasComment("UTC timestamp when the row was created.")
                .HasDefaultValueSql("(sysutcdatetime())", "DF_TransactionDetails_CreatedUtc");
            entity.Property(e => e.IsSynthetic)
                .HasComment("Indicates whether the row is synthetic/demo data.")
                .HasDefaultValue(true, "DF_TransactionDetails_IsSynthetic");
            entity.Property(e => e.LineTotal).HasComment("Extended amount for the line item.");
            entity.Property(e => e.OrderId).HasComment("Reference to the parent order.");
            entity.Property(e => e.ProductId).HasComment("Reference to the product sold.");
            entity.Property(e => e.Quantity).HasComment("Quantity sold for this line item.");
            entity.Property(e => e.ScenarioName).HasComment("Optional simulation scenario label.");
            entity.Property(e => e.UnitPrice).HasComment("Unit selling price used on the transaction.");

            entity.HasOne(d => d.Order).WithMany(p => p.TransactionDetails).OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionDetails).OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
