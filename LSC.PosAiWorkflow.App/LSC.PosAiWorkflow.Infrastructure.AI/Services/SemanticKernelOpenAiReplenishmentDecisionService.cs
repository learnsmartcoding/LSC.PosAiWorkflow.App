using System.Text.Json;
using LSC.PosAiWorkflow.Application.Abstractions.AI;
using LSC.PosAiWorkflow.Application.Inventory.Dtos;
using LSC.PosAiWorkflow.Application.Replenishment.Dtos;
using LSC.PosAiWorkflow.Infrastructure.AI.Configuration;
using LSC.PosAiWorkflow.Infrastructure.AI.Models;
using LSC.PosAiWorkflow.Infrastructure.AI.Prompts;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel.ChatCompletion;

namespace LSC.PosAiWorkflow.Infrastructure.AI.Services;

public sealed class SemanticKernelOpenAiReplenishmentDecisionService : IReplenishmentDecisionService
{
    private readonly IChatCompletionService _chatCompletionService;
    private readonly OpenAiOptions _openAiOptions;

    public SemanticKernelOpenAiReplenishmentDecisionService(
        IChatCompletionService chatCompletionService,
        IOptions<OpenAiOptions> openAiOptions)
    {
        _chatCompletionService = chatCompletionService;
        _openAiOptions = openAiOptions.Value;
    }

    public async Task<ReplenishmentDecisionResultDto> EvaluateAsync(
        InventorySnapshotDto inventory,
        SalesVelocityDto salesVelocity,
        string? scenarioName,
        CancellationToken cancellationToken = default)
    {
        var prompt = ReplenishmentPromptBuilder.Build(inventory, salesVelocity, scenarioName);

        var chatHistory = new ChatHistory();
        chatHistory.AddSystemMessage("You are a precise inventory replenishment decision engine.");
        chatHistory.AddUserMessage(prompt);

        var response = await _chatCompletionService.GetChatMessageContentAsync(
            chatHistory,
            cancellationToken: cancellationToken);

        var rawText = response.Content?.Trim();

        if (string.IsNullOrWhiteSpace(rawText))
        {
            throw new InvalidOperationException("OpenAI returned an empty response.");
        }

        var parsed = ParseResponse(rawText);

        var estimatedUnitCost = inventory.CostPrice;
        var estimatedTotalCost = estimatedUnitCost * parsed.SuggestedQuantity;

        return new ReplenishmentDecisionResultDto
        {
            ReorderNeeded = parsed.ReorderNeeded,
            SuggestedQuantity = parsed.RecommendedAction == "NoAction" ? 0 : parsed.SuggestedQuantity,
            Reasoning = parsed.Reasoning,
            RecommendedAction = parsed.RecommendedAction,
            ConfidenceScore = parsed.ConfidenceScore,
            RiskLevel = parsed.RiskLevel,
            EstimatedUnitCost = estimatedUnitCost,
            EstimatedTotalCost = estimatedTotalCost,
            RequiresApproval = parsed.RequiresApproval || estimatedTotalCost >= 5000m,
            InputSummaryJson = JsonSerializer.Serialize(new
            {
                inventory.ProductId,
                inventory.ProductName,
                inventory.Sku,
                inventory.StoreCode,
                inventory.QuantityOnHand,
                inventory.QuantityReserved,
                inventory.QuantityAvailable,
                inventory.ReorderThreshold,
                inventory.PreferredReorderQuantity,
                inventory.LeadTimeDays,
                inventory.CostPrice,
                inventory.UnitPrice,
                salesVelocity.DaysConsidered,
                salesVelocity.TotalUnitsSold,
                salesVelocity.AverageUnitsPerDay,
                scenarioName
            }),
            DecisionJson = JsonSerializer.Serialize(new
            {
                parsed.ReorderNeeded,
                SuggestedQuantity = parsed.RecommendedAction == "NoAction" ? 0 : parsed.SuggestedQuantity,
                parsed.Reasoning,
                parsed.RecommendedAction,
                parsed.ConfidenceScore,
                parsed.RiskLevel,
                RequiresApproval = parsed.RequiresApproval || estimatedTotalCost >= 5000m,
                estimatedUnitCost,
                estimatedTotalCost,
                scenarioName,
                provider = "OpenAI",
                model = _openAiOptions.ModelId
            })
        };
    }

    private static ReplenishmentDecisionResponseModel ParseResponse(string rawText)
    {
        var cleaned = CleanupJson(rawText);

        var parsed = JsonSerializer.Deserialize<ReplenishmentDecisionResponseModel>(
            cleaned,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (parsed is null)
        {
            throw new InvalidOperationException("Failed to deserialize OpenAI replenishment response.");
        }

        if (string.IsNullOrWhiteSpace(parsed.RecommendedAction))
        {
            parsed.RecommendedAction = parsed.ReorderNeeded ? "CreatePurchaseOrderDraft" : "NoAction";
        }

        if (parsed.RecommendedAction != "CreatePurchaseOrderDraft" && parsed.RecommendedAction != "NoAction")
        {
            parsed.RecommendedAction = parsed.ReorderNeeded ? "CreatePurchaseOrderDraft" : "NoAction";
        }

        parsed.RiskLevel = NormalizeRiskLevel(parsed.RiskLevel);
        parsed.ConfidenceScore = NormalizeConfidence(parsed.ConfidenceScore);

        if (!parsed.ReorderNeeded)
        {
            parsed.SuggestedQuantity = 0;
            parsed.RecommendedAction = "NoAction";
        }
        else if (parsed.SuggestedQuantity <= 0)
        {
            parsed.SuggestedQuantity = 1;
        }

        parsed.Reasoning = string.IsNullOrWhiteSpace(parsed.Reasoning)
            ? "Decision generated by OpenAI."
            : parsed.Reasoning.Trim();

        return parsed;
    }

    private static string CleanupJson(string rawText)
    {
        var cleaned = rawText.Trim();

        if (cleaned.StartsWith("```"))
        {
            cleaned = cleaned.Replace("```json", "", StringComparison.OrdinalIgnoreCase)
                             .Replace("```", "")
                             .Trim();
        }

        var firstBrace = cleaned.IndexOf('{');
        var lastBrace = cleaned.LastIndexOf('}');

        if (firstBrace >= 0 && lastBrace > firstBrace)
        {
            cleaned = cleaned[firstBrace..(lastBrace + 1)];
        }

        return cleaned;
    }

    private static string NormalizeRiskLevel(string? riskLevel)
    {
        if (string.IsNullOrWhiteSpace(riskLevel))
        {
            return "Low";
        }

        var normalized = riskLevel.Trim().ToLowerInvariant();

        return normalized switch
        {
            "low" => "Low",
            "medium" => "Medium",
            "high" => "High",
            _ => "Low"
        };
    }

    private static decimal NormalizeConfidence(decimal? confidence)
    {
        if (!confidence.HasValue)
        {
            return 75m;
        }

        if (confidence.Value < 0) return 0m;
        if (confidence.Value > 100) return 100m;

        return confidence.Value;
    }
}