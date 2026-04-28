using LSC.PosAiWorkflow.Api.Configuration;
using LSC.PosAiWorkflow.Api.Simulation;
using Microsoft.Extensions.Options;

namespace LSC.PosAiWorkflow.Api.BackgroundServices;

public sealed class CatalogBootstrapBackgroundService : BackgroundService
{
    private readonly SimulationDataService _simulationDataService;
    private readonly SimulationOptions _options;
    private readonly ILogger<CatalogBootstrapBackgroundService> _logger;

    public CatalogBootstrapBackgroundService(
        SimulationDataService simulationDataService,
        IOptions<SimulationOptions> options,
        ILogger<CatalogBootstrapBackgroundService> logger)
    {
        _simulationDataService = simulationDataService;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableCatalogBootstrap)
        {
            _logger.LogInformation("Catalog bootstrap simulation is disabled.");
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _simulationDataService.EnsureCatalogAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while bootstrapping catalog.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_options.CatalogBootstrapIntervalSeconds),
                stoppingToken);
        }
    }
}