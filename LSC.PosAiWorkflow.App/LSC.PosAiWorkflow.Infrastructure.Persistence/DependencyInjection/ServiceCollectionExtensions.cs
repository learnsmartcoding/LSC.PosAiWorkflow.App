using LSC.PosAiWorkflow.Application.Abstractions.Persistence;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Context;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Repositories;
using LSC.PosAiWorkflow.Infrastructure.Persistence.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PosAiWorkflowDb");

        services.AddDbContext<PosAiWorkflowSimDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IInventoryReadService, InventoryReadService>();
        services.AddScoped<IAiDecisionRepository, AiDecisionRepository>();
        services.AddScoped<IApprovalRequestRepository, ApprovalRequestRepository>();
        services.AddScoped<IPurchaseOrderDraftRepository, PurchaseOrderDraftRepository>();
        services.AddScoped<ISystemEventRepository, SystemEventRepository>();

        services.AddScoped<IAiDecisionQueryService, AiDecisionQueryService>();
        services.AddScoped<IApprovalRequestQueryService, ApprovalRequestQueryService>();
        services.AddScoped<IPurchaseOrderDraftQueryService, PurchaseOrderDraftQueryService>();
        services.AddScoped<ISystemEventQueryService, SystemEventQueryService>();

        services.AddScoped<IApprovalWorkflowQueryService, ApprovalWorkflowQueryService>();
        services.AddScoped<IApprovalWorkflowRepository, ApprovalWorkflowRepository>();

        return services;
    }
}