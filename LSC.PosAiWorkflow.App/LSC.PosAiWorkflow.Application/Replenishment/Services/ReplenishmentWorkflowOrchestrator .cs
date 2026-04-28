using LSC.PosAiWorkflow.Application.Abstractions.AI;
using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Application.Replenishment.Dtos;
using System.Text.Json;

namespace LSC.PosAiWorkflow.Application.Replenishment.Services;

public sealed class ReplenishmentWorkflowOrchestrator : IReplenishmentWorkflowOrchestrator
{
    private const decimal ApprovalThresholdAmount = 5000m;

    private readonly IInventoryReadService _inventoryReadService;
    private readonly IReplenishmentDecisionService _replenishmentDecisionService;
    private readonly IAiDecisionRepository _aiDecisionRepository;
    private readonly IApprovalRequestRepository _approvalRequestRepository;
    private readonly IPurchaseOrderDraftRepository _purchaseOrderDraftRepository;
    private readonly ISystemEventRepository _systemEventRepository;

    public ReplenishmentWorkflowOrchestrator(
        IInventoryReadService inventoryReadService,
        IReplenishmentDecisionService replenishmentDecisionService,
        IAiDecisionRepository aiDecisionRepository,
        IApprovalRequestRepository approvalRequestRepository,
        IPurchaseOrderDraftRepository purchaseOrderDraftRepository,
        ISystemEventRepository systemEventRepository)
    {
        _inventoryReadService = inventoryReadService;
        _replenishmentDecisionService = replenishmentDecisionService;
        _aiDecisionRepository = aiDecisionRepository;
        _approvalRequestRepository = approvalRequestRepository;
        _purchaseOrderDraftRepository = purchaseOrderDraftRepository;
        _systemEventRepository = systemEventRepository;
    }

    public async Task<long> EvaluateAsync(ReplenishmentDecisionRequestDto request, CancellationToken cancellationToken = default)
    {
        var correlationId = Guid.NewGuid();

        var inventory = await _inventoryReadService.GetInventorySnapshotAsync(
            request.ProductId,
            request.StoreCode,
            cancellationToken);

        if (inventory is null)
        {
            throw new InvalidOperationException($"Inventory not found for ProductId={request.ProductId}, StoreCode={request.StoreCode}.");
        }

        var salesVelocity = await _inventoryReadService.GetSalesVelocityAsync(
            request.ProductId,
            days: 7,
            cancellationToken);

        await _systemEventRepository.CreateAsync(new SystemEventCreateModel
        {
            CorrelationId = correlationId,
            EventType = "ReplenishmentEvaluationStarted",
            EntityType = "Product",
            EntityId = request.ProductId,
            EventDataJson = JsonSerializer.Serialize(new
            {
                request.ProductId,
                request.StoreCode,
                request.ScenarioName
            }),
            ScenarioName = request.ScenarioName,
            IsSynthetic = true,
            CreatedBy = "Application"
        }, cancellationToken);

        var decision = await _replenishmentDecisionService.EvaluateAsync(
            inventory,
            salesVelocity,
            request.ScenarioName,
            cancellationToken);

        var decisionStatus = decision.RequiresApproval ? "PendingApproval" : "AutoApproved";

        var aiDecisionId = await _aiDecisionRepository.CreateAsync(new AiDecisionCreateModel
        {
            DecisionType = "ReplenishmentRecommendation",
            EntityType = "Product",
            EntityId = request.ProductId,
            ProductId = request.ProductId,
            StoreCode = request.StoreCode,
            CorrelationId = correlationId,
            ModelName = "configured-provider",
            PromptVersion = "v1",
            InputSummaryJson = decision.InputSummaryJson,
            DecisionJson = decision.DecisionJson,
            RecommendedAction = decision.RecommendedAction,
            ConfidenceScore = decision.ConfidenceScore,
            RiskLevel = decision.RiskLevel,
            DecisionStatus = decisionStatus,
            SuggestedQuantity = decision.SuggestedQuantity,
            EstimatedUnitCost = decision.EstimatedUnitCost,
            EstimatedTotalCost = decision.EstimatedTotalCost,
            RequiresApproval = decision.RequiresApproval,
            Reasoning = decision.Reasoning,
            ScenarioName = request.ScenarioName,
            IsSynthetic = true
        }, cancellationToken);

        await _systemEventRepository.CreateAsync(new SystemEventCreateModel
        {
            CorrelationId = correlationId,
            EventType = "AiDecisionCreated",
            EntityType = "AiDecision",
            EntityId = aiDecisionId,
            EventDataJson = decision.DecisionJson,
            ScenarioName = request.ScenarioName,
            IsSynthetic = true,
            CreatedBy = "Application"
        }, cancellationToken);

        if (!decision.ReorderNeeded)
        {
            await _systemEventRepository.CreateAsync(new SystemEventCreateModel
            {
                CorrelationId = correlationId,
                EventType = "NoReorderNeeded",
                EntityType = "AiDecision",
                EntityId = aiDecisionId,
                EventDataJson = decision.DecisionJson,
                ScenarioName = request.ScenarioName,
                IsSynthetic = true,
                CreatedBy = "Application"
            }, cancellationToken);

            return aiDecisionId;
        }

        if (decision.EstimatedTotalCost >= ApprovalThresholdAmount || decision.RequiresApproval)
        {
            await _approvalRequestRepository.CreateAsync(new ApprovalRequestCreateModel
            {
                AiDecisionId = aiDecisionId,
                ApprovalType = "RestockApproval",
                Status = "Pending",
                RequestedBy = "AI-Agent",
                ScenarioName = request.ScenarioName,
                IsSynthetic = true
            }, cancellationToken);

            await _systemEventRepository.CreateAsync(new SystemEventCreateModel
            {
                CorrelationId = correlationId,
                EventType = "ApprovalRequested",
                EntityType = "AiDecision",
                EntityId = aiDecisionId,
                EventDataJson = JsonSerializer.Serialize(new
                {
                    decision.SuggestedQuantity,
                    decision.EstimatedUnitCost,
                    decision.EstimatedTotalCost
                }),
                ScenarioName = request.ScenarioName,
                IsSynthetic = true,
                CreatedBy = "Application"
            }, cancellationToken);
        }
        else
        {
            await _purchaseOrderDraftRepository.CreateAsync(new PurchaseOrderDraftCreateModel
            {
                AiDecisionId = aiDecisionId,
                ProductId = request.ProductId,
                StoreCode = request.StoreCode,
                SuggestedQuantity = decision.SuggestedQuantity,
                EstimatedUnitCost = decision.EstimatedUnitCost,
                EstimatedTotalCost = decision.EstimatedTotalCost,
                DraftStatus = "Draft",
                Notes = decision.Reasoning,
                ScenarioName = request.ScenarioName,
                IsSynthetic = true
            }, cancellationToken);

            await _systemEventRepository.CreateAsync(new SystemEventCreateModel
            {
                CorrelationId = correlationId,
                EventType = "PurchaseOrderDraftCreated",
                EntityType = "AiDecision",
                EntityId = aiDecisionId,
                EventDataJson = JsonSerializer.Serialize(new
                {
                    decision.SuggestedQuantity,
                    decision.EstimatedTotalCost
                }),
                ScenarioName = request.ScenarioName,
                IsSynthetic = true,
                CreatedBy = "Application"
            }, cancellationToken);
        }

        return aiDecisionId;
    }
}