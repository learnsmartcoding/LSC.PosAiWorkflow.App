namespace LSC.PosAiWorkflow.Api.Configuration;

public sealed class SimulationOptions
{
    public bool EnableCatalogBootstrap { get; set; } = true;
    public bool EnableOrderSimulation { get; set; } = true;

    public int TargetProductCount { get; set; } = 100;

    public int CatalogBootstrapIntervalSeconds { get; set; } = 300;
    public int OrderSimulationIntervalSeconds { get; set; } = 10;

    public int MaxOrdersPerCycle { get; set; } = 3;
    public int MaxLineItemsPerOrder { get; set; } = 4;

    public string DefaultStoreCode { get; set; } = "STORE001";

    public bool EnableAutoReplenishmentTrigger { get; set; } = true;
    public int ReplenishmentCooldownMinutes { get; set; } = 10;
    public int ReplenishmentTriggerBuffer { get; set; } = 5;
}