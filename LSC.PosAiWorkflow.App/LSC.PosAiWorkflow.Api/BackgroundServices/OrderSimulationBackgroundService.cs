using LSC.PosAiWorkflow.Api.Configuration;
using LSC.PosAiWorkflow.Api.Simulation;
using Microsoft.Extensions.Options;

namespace LSC.PosAiWorkflow.Api.BackgroundServices;

public sealed class OrderSimulationBackgroundService : BackgroundService
{
    private readonly SimulationDataService _simulationDataService;
    private readonly SimulationOptions _options;
    private readonly ILogger<OrderSimulationBackgroundService> _logger;

    public OrderSimulationBackgroundService(
        SimulationDataService simulationDataService,
        IOptions<SimulationOptions> options,
        ILogger<OrderSimulationBackgroundService> logger)
    {
        _simulationDataService = simulationDataService;
        _options = options.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_options.EnableOrderSimulation)
        {
            _logger.LogInformation("Order simulation is disabled.");
            return;
        }

        await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await _simulationDataService.SimulateOrdersAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while simulating orders.");
            }

            await Task.Delay(
                TimeSpan.FromSeconds(_options.OrderSimulationIntervalSeconds),
                stoppingToken);
        }
    }
}