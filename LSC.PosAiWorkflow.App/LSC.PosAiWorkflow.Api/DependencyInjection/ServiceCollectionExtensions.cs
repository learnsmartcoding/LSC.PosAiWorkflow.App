//using LSC.PosAiWorkflow.Application.Abstractions.AI;
//using LSC.PosAiWorkflow.Application.Abstractions.Services;
//using LSC.PosAiWorkflow.Application.Approvals.Services;
//using LSC.PosAiWorkflow.Application.Replenishment.Services;
//using LSC.PosAiWorkflow.Infrastructure.AI;
//using LSC.PosAiWorkflow.Infrastructure.Persistence.DependencyInjection;

//namespace LSC.PosAiWorkflow.Api.DependencyInjection;

//public static class ServiceCollectionExtensions
//{
//    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
//    {
//        services.AddPersistence(configuration);
//        services.AddAiServices(configuration);

//        services.AddScoped<IReplenishmentDecisionService, FakeReplenishmentDecisionService>();
//        services.AddScoped<IReplenishmentWorkflowOrchestrator, ReplenishmentWorkflowOrchestrator>();
//        services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();

//        return services;
//    }
//}


using LSC.PosAiWorkflow.Application.Abstractions.Services;
using LSC.PosAiWorkflow.Application.Approvals.Services;
using LSC.PosAiWorkflow.Application.Replenishment.Services;
using LSC.PosAiWorkflow.Infrastructure.AI.DependencyInjection;
using LSC.PosAiWorkflow.Infrastructure.Persistence.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LSC.PosAiWorkflow.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddAiServices(configuration);

        services.AddScoped<IReplenishmentWorkflowOrchestrator, ReplenishmentWorkflowOrchestrator>();
        services.AddScoped<IApprovalWorkflowService, ApprovalWorkflowService>();

        return services;
    }
}