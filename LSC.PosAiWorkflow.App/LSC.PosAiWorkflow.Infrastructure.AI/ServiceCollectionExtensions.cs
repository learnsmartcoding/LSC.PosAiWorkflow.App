using LSC.PosAiWorkflow.Application.Abstractions.AI;
using LSC.PosAiWorkflow.Infrastructure.AI.Configuration;
using LSC.PosAiWorkflow.Infrastructure.AI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace LSC.PosAiWorkflow.Infrastructure.AI.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AiProviderOptions>(configuration.GetSection("AiProvider"));
        services.Configure<OllamaOptions>(configuration.GetSection("Ollama"));
        services.Configure<OpenAiOptions>(configuration.GetSection("OpenAI"));

        services.AddKeyedSingleton<IChatCompletionService>("ollama", (sp, _) =>
        {
            var ollamaOptions = sp.GetRequiredService<IOptions<OllamaOptions>>().Value;

            var kernelBuilder = Kernel.CreateBuilder();

            #pragma warning disable SKEXP0070
                        kernelBuilder.AddOllamaChatCompletion(
                            modelId: ollamaOptions.ModelId,
                            endpoint: new Uri(ollamaOptions.Endpoint),
                            serviceId: ollamaOptions.ServiceId);
            #pragma warning restore SKEXP0070

            var kernel = kernelBuilder.Build();
            return kernel.GetRequiredService<IChatCompletionService>();
        });

        services.AddKeyedSingleton<IChatCompletionService>("openai", (sp, _) =>
        {
            var openAiOptions = sp.GetRequiredService<IOptions<OpenAiOptions>>().Value;

            var kernelBuilder = Kernel.CreateBuilder();
            kernelBuilder.AddOpenAIChatCompletion(
                modelId: openAiOptions.ModelId,
                apiKey: openAiOptions.ApiKey,
                orgId: string.IsNullOrWhiteSpace(openAiOptions.OrgId) ? null : openAiOptions.OrgId,
                serviceId: openAiOptions.ServiceId);

            var kernel = kernelBuilder.Build();
            return kernel.GetRequiredService<IChatCompletionService>();
        });

        services.AddScoped(sp =>
            new SemanticKernelOllamaReplenishmentDecisionService(
                sp.GetRequiredKeyedService<IChatCompletionService>("ollama"),
                sp.GetRequiredService<IOptions<OllamaOptions>>()));

        services.AddScoped(sp =>
            new SemanticKernelOpenAiReplenishmentDecisionService(
                sp.GetRequiredKeyedService<IChatCompletionService>("openai"),
                sp.GetRequiredService<IOptions<OpenAiOptions>>()));

        services.AddScoped<FakeReplenishmentDecisionService>();

        services.AddScoped<IReplenishmentDecisionService>(sp =>
        {
            var providerOptions = sp.GetRequiredService<IOptions<AiProviderOptions>>().Value;

            return providerOptions.Provider.ToLowerInvariant() switch
            {
                "ollama" => sp.GetRequiredService<SemanticKernelOllamaReplenishmentDecisionService>(),
                "openai" => sp.GetRequiredService<SemanticKernelOpenAiReplenishmentDecisionService>(),
                _ => sp.GetRequiredService<FakeReplenishmentDecisionService>()
            };
        });

        return services;
    }
}