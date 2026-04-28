namespace LSC.PosAiWorkflow.Api.Simulation;

public sealed class ProductTemplate
{
    public string CategoryName { get; init; } = string.Empty;
    public string CategoryCode { get; init; } = string.Empty;
    public string Sku { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public decimal CostPrice { get; init; }
    public int ReorderThreshold { get; init; }
    public int PreferredReorderQuantity { get; init; }
    public int LeadTimeDays { get; init; }
    public int InitialQuantityOnHand { get; init; }
    public int InitialQuantityReserved { get; init; }
    public int DemandWeight { get; init; }
}

public static class ProductCatalogTemplateProvider
{
    public static IReadOnlyList<ProductTemplate> GetTemplates() => new List<ProductTemplate>
    {
        new() { CategoryName = "Groceries", CategoryCode = "GROC", Sku = "RICE-5KG-001", Name = "Premium Rice 5KG", Description = "High-demand staple", UnitPrice = 18.99m, CostPrice = 12.50m, ReorderThreshold = 20, PreferredReorderQuantity = 80, LeadTimeDays = 5, InitialQuantityOnHand = 60, InitialQuantityReserved = 5, DemandWeight = 10 },
        new() { CategoryName = "Groceries", CategoryCode = "GROC", Sku = "OIL-1L-001", Name = "Cooking Oil 1L", Description = "Daily essential", UnitPrice = 9.49m, CostPrice = 6.10m, ReorderThreshold = 15, PreferredReorderQuantity = 50, LeadTimeDays = 4, InitialQuantityOnHand = 80, InitialQuantityReserved = 5, DemandWeight = 8 },
        new() { CategoryName = "Snacks", CategoryCode = "SNCK", Sku = "BAR-12PK-001", Name = "Protein Bars Pack", Description = "Higher cost snack item", UnitPrice = 39.99m, CostPrice = 25.00m, ReorderThreshold = 10, PreferredReorderQuantity = 40, LeadTimeDays = 7, InitialQuantityOnHand = 35, InitialQuantityReserved = 2, DemandWeight = 5 },
        new() { CategoryName = "Dairy", CategoryCode = "DAIR", Sku = "MILK-1G-001", Name = "Whole Milk 1 Gallon", Description = "Fast moving dairy", UnitPrice = 5.49m, CostPrice = 3.75m, ReorderThreshold = 18, PreferredReorderQuantity = 45, LeadTimeDays = 2, InitialQuantityOnHand = 50, InitialQuantityReserved = 3, DemandWeight = 9 },
        new() { CategoryName = "Bakery", CategoryCode = "BAKE", Sku = "BREAD-WHT-001", Name = "Whole Wheat Bread", Description = "Daily bread item", UnitPrice = 3.99m, CostPrice = 2.10m, ReorderThreshold = 20, PreferredReorderQuantity = 40, LeadTimeDays = 2, InitialQuantityOnHand = 55, InitialQuantityReserved = 4, DemandWeight = 9 },
        new() { CategoryName = "Household", CategoryCode = "HSHD", Sku = "SOAP-6PK-001", Name = "Bath Soap 6 Pack", Description = "Household essentials", UnitPrice = 7.99m, CostPrice = 4.20m, ReorderThreshold = 12, PreferredReorderQuantity = 30, LeadTimeDays = 6, InitialQuantityOnHand = 40, InitialQuantityReserved = 2, DemandWeight = 4 },
        new() { CategoryName = "Beverages", CategoryCode = "BEVG", Sku = "SODA-12PK-001", Name = "Soft Drink 12 Pack", Description = "Popular beverage", UnitPrice = 8.99m, CostPrice = 5.50m, ReorderThreshold = 16, PreferredReorderQuantity = 36, LeadTimeDays = 4, InitialQuantityOnHand = 60, InitialQuantityReserved = 3, DemandWeight = 7 },
        new() { CategoryName = "Pantry", CategoryCode = "PNTR", Sku = "PASTA-1LB-001", Name = "Pasta 1LB", Description = "Pantry staple", UnitPrice = 2.49m, CostPrice = 1.30m, ReorderThreshold = 25, PreferredReorderQuantity = 70, LeadTimeDays = 5, InitialQuantityOnHand = 75, InitialQuantityReserved = 4, DemandWeight = 8 },
        new() { CategoryName = "Cleaning", CategoryCode = "CLNG", Sku = "DETERG-001", Name = "Laundry Detergent", Description = "Cleaning supply", UnitPrice = 14.99m, CostPrice = 9.20m, ReorderThreshold = 10, PreferredReorderQuantity = 25, LeadTimeDays = 6, InitialQuantityOnHand = 30, InitialQuantityReserved = 1, DemandWeight = 3 }
    };
}