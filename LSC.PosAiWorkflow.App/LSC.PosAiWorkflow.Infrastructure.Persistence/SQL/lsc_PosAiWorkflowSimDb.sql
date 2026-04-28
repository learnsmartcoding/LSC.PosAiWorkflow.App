
CREATE DATABASE PosAiWorkflowSimDb
GO


USE PosAiWorkflowSimDb

GO
--PosAiWorkflowSimDb
/*
====================================================================================================
Project: AI Replenishment Workflow Simulator
Purpose: Minimal schema for a realistic AI-powered inventory replenishment workflow demo
Database: SQL Server
Notes:
- Uses named constraints
- Adds table/column comments through SQL Server extended properties
- Includes ScenarioName and IsSynthetic for simulation/demo support
====================================================================================================
*/

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

/*==================================================================================================
  CATEGORY
==================================================================================================*/
CREATE TABLE dbo.Category
(
    CategoryId              BIGINT IDENTITY(1,1) NOT NULL,
    Name                    NVARCHAR(100) NOT NULL,
    Code                    NVARCHAR(30) NOT NULL,
    Description             NVARCHAR(250) NULL,
    IsActive                BIT NOT NULL
        CONSTRAINT DF_Category_IsActive DEFAULT (1),
    ScenarioName            NVARCHAR(100) NULL,
    IsSynthetic             BIT NOT NULL
        CONSTRAINT DF_Category_IsSynthetic DEFAULT (1),
    CreatedUtc              DATETIME2(3) NOT NULL
        CONSTRAINT DF_Category_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedUtc              DATETIME2(3) NULL,

    CONSTRAINT PK_Category PRIMARY KEY CLUSTERED (CategoryId),
    CONSTRAINT UQ_Category_Name UNIQUE (Name),
    CONSTRAINT UQ_Category_Code UNIQUE (Code)
);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores product categories used to group products for inventory and replenishment analysis.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'Category';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for category.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'CategoryId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Human-readable category name.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'Name';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Short business code for the category.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'Code';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional category description.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'Description';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the category is active.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'IsActive';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label such as HolidaySpike or NormalSales.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row was created from synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was last updated.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Category', @level2type=N'COLUMN', @level2name=N'UpdatedUtc';
GO

/*==================================================================================================
  PRODUCT
==================================================================================================*/
CREATE TABLE dbo.Product
(
    ProductId                    BIGINT IDENTITY(1,1) NOT NULL,
    CategoryId                   BIGINT NOT NULL,
    Sku                          NVARCHAR(50) NOT NULL,
    Name                         NVARCHAR(150) NOT NULL,
    Description                  NVARCHAR(500) NULL,
    UnitPrice                    DECIMAL(18,2) NOT NULL,
    CostPrice                    DECIMAL(18,2) NOT NULL,
    ReorderThreshold             INT NOT NULL,
    PreferredReorderQuantity     INT NOT NULL,
    LeadTimeDays                 INT NOT NULL,
    IsActive                     BIT NOT NULL
        CONSTRAINT DF_Product_IsActive DEFAULT (1),
    ScenarioName                 NVARCHAR(100) NULL,
    IsSynthetic                  BIT NOT NULL
        CONSTRAINT DF_Product_IsSynthetic DEFAULT (1),
    CreatedUtc                   DATETIME2(3) NOT NULL
        CONSTRAINT DF_Product_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedUtc                   DATETIME2(3) NULL,

    CONSTRAINT PK_Product PRIMARY KEY CLUSTERED (ProductId),
    CONSTRAINT FK_Product_Category_CategoryId
        FOREIGN KEY (CategoryId) REFERENCES dbo.Category(CategoryId),
    CONSTRAINT UQ_Product_Sku UNIQUE (Sku),
    CONSTRAINT CK_Product_UnitPrice_NonNegative CHECK (UnitPrice >= 0),
    CONSTRAINT CK_Product_CostPrice_NonNegative CHECK (CostPrice >= 0),
    CONSTRAINT CK_Product_ReorderThreshold_Positive CHECK (ReorderThreshold >= 0),
    CONSTRAINT CK_Product_PreferredReorderQuantity_Positive CHECK (PreferredReorderQuantity > 0),
    CONSTRAINT CK_Product_LeadTimeDays_NonNegative CHECK (LeadTimeDays >= 0)
);
GO

CREATE NONCLUSTERED INDEX IX_Product_CategoryId
    ON dbo.Product(CategoryId);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores product master data used by sales, inventory, and AI replenishment decisioning.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'Product';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for product.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'ProductId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the category the product belongs to.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'CategoryId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique stock keeping unit.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'Sku';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Human-readable product name.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'Name';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional product description.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'Description';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Selling price per unit.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'UnitPrice';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estimated cost price per unit used for replenishment value calculation.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'CostPrice';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Base stock threshold below which replenishment should be evaluated.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'ReorderThreshold';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Default reorder quantity suggestion before AI adjusts it.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'PreferredReorderQuantity';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estimated supplier lead time in days.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'LeadTimeDays';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the product is active.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'IsActive';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was last updated.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Product', @level2type=N'COLUMN', @level2name=N'UpdatedUtc';
GO

/*==================================================================================================
  STORE INVENTORY
==================================================================================================*/
CREATE TABLE dbo.StoreInventory
(
    StoreInventoryId         BIGINT IDENTITY(1,1) NOT NULL,
    ProductId                BIGINT NOT NULL,
    StoreCode                NVARCHAR(30) NOT NULL,
    QuantityOnHand           INT NOT NULL,
    QuantityReserved         INT NOT NULL
        CONSTRAINT DF_StoreInventory_QuantityReserved DEFAULT (0),
    LastStockUpdatedUtc      DATETIME2(3) NOT NULL
        CONSTRAINT DF_StoreInventory_LastStockUpdatedUtc DEFAULT (SYSUTCDATETIME()),
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_StoreInventory_IsSynthetic DEFAULT (1),
    CreatedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_StoreInventory_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    UpdatedUtc               DATETIME2(3) NULL,

    CONSTRAINT PK_StoreInventory PRIMARY KEY CLUSTERED (StoreInventoryId),
    CONSTRAINT FK_StoreInventory_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId),
    CONSTRAINT UQ_StoreInventory_ProductId_StoreCode UNIQUE (ProductId, StoreCode),
    CONSTRAINT CK_StoreInventory_QuantityOnHand_NonNegative CHECK (QuantityOnHand >= 0),
    CONSTRAINT CK_StoreInventory_QuantityReserved_NonNegative CHECK (QuantityReserved >= 0),
    CONSTRAINT CK_StoreInventory_Reserved_LessEqual_OnHand CHECK (QuantityReserved <= QuantityOnHand)
);
GO

CREATE NONCLUSTERED INDEX IX_StoreInventory_ProductId
    ON dbo.StoreInventory(ProductId);

CREATE NONCLUSTERED INDEX IX_StoreInventory_StoreCode
    ON dbo.StoreInventory(StoreCode);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores current stock position for each product by store, used by the AI replenishment workflow.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'StoreInventory';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for store inventory row.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'StoreInventoryId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the product in stock.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'ProductId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Business code for the store or location.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'StoreCode';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total physical units currently on hand.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'QuantityOnHand';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Units reserved and not freely available for sale.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'QuantityReserved';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp for the most recent stock update.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'LastStockUpdatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was last updated.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'StoreInventory', @level2type=N'COLUMN', @level2name=N'UpdatedUtc';
GO

/*==================================================================================================
  ORDERS
==================================================================================================*/
CREATE TABLE dbo.Orders
(
    OrderId                  BIGINT IDENTITY(1,1) NOT NULL,
    OrderNumber              NVARCHAR(40) NOT NULL,
    OrderUtc                 DATETIME2(3) NOT NULL,
    CustomerName             NVARCHAR(150) NULL,
    OrderStatus              NVARCHAR(30) NOT NULL,
    TotalAmount              DECIMAL(18,2) NOT NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_Orders_IsSynthetic DEFAULT (1),
    CreatedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_Orders_CreatedUtc DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_Orders PRIMARY KEY CLUSTERED (OrderId),
    CONSTRAINT UQ_Orders_OrderNumber UNIQUE (OrderNumber),
    CONSTRAINT CK_Orders_TotalAmount_NonNegative CHECK (TotalAmount >= 0),
    CONSTRAINT CK_Orders_OrderStatus_Valid CHECK (OrderStatus IN (N'Pending', N'Completed', N'Cancelled'))
);
GO

CREATE NONCLUSTERED INDEX IX_Orders_OrderUtc
    ON dbo.Orders(OrderUtc);

CREATE NONCLUSTERED INDEX IX_Orders_OrderStatus
    ON dbo.Orders(OrderStatus);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores order headers representing completed or simulated sales transactions.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'Orders';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for order header.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'OrderId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique business-facing order number.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'OrderNumber';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the order was placed.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'OrderUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional customer name for demo readability.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'CustomerName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current order status such as Pending, Completed, or Cancelled.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'OrderStatus';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Total monetary value of the order.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'TotalAmount';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'Orders', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
GO

/*==================================================================================================
  TRANSACTION DETAILS
==================================================================================================*/
CREATE TABLE dbo.TransactionDetails
(
    TransactionDetailId      BIGINT IDENTITY(1,1) NOT NULL,
    OrderId                  BIGINT NOT NULL,
    ProductId                BIGINT NOT NULL,
    Quantity                 INT NOT NULL,
    UnitPrice                DECIMAL(18,2) NOT NULL,
    LineTotal                DECIMAL(18,2) NOT NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_TransactionDetails_IsSynthetic DEFAULT (1),
    CreatedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_TransactionDetails_CreatedUtc DEFAULT (SYSUTCDATETIME()),

    CONSTRAINT PK_TransactionDetails PRIMARY KEY CLUSTERED (TransactionDetailId),
    CONSTRAINT FK_TransactionDetails_Orders_OrderId
        FOREIGN KEY (OrderId) REFERENCES dbo.Orders(OrderId),
    CONSTRAINT FK_TransactionDetails_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId),
    CONSTRAINT CK_TransactionDetails_Quantity_Positive CHECK (Quantity > 0),
    CONSTRAINT CK_TransactionDetails_UnitPrice_NonNegative CHECK (UnitPrice >= 0),
    CONSTRAINT CK_TransactionDetails_LineTotal_NonNegative CHECK (LineTotal >= 0)
);
GO

CREATE NONCLUSTERED INDEX IX_TransactionDetails_OrderId
    ON dbo.TransactionDetails(OrderId);

CREATE NONCLUSTERED INDEX IX_TransactionDetails_ProductId
    ON dbo.TransactionDetails(ProductId);

CREATE NONCLUSTERED INDEX IX_TransactionDetails_ProductId_CreatedUtc
    ON dbo.TransactionDetails(ProductId, CreatedUtc);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores order line items used to calculate sales velocity and consumption trends for replenishment.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'TransactionDetails';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for the order line item.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'TransactionDetailId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the parent order.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'OrderId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the product sold.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'ProductId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quantity sold for this line item.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'Quantity';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unit selling price used on the transaction.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'UnitPrice';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Extended amount for the line item.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'LineTotal';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the row was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'TransactionDetails', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
GO

/*==================================================================================================
  INVENTORY AUDIT LOG
==================================================================================================*/
CREATE TABLE dbo.InventoryAuditLog
(
    InventoryAuditLogId      BIGINT IDENTITY(1,1) NOT NULL,
    ProductId                BIGINT NOT NULL,
    StoreCode                NVARCHAR(30) NOT NULL,
    ChangeType               NVARCHAR(30) NOT NULL,
    QuantityBefore           INT NOT NULL,
    QuantityChanged          INT NOT NULL,
    QuantityAfter            INT NOT NULL,
    ReferenceType            NVARCHAR(30) NULL,
    ReferenceId              BIGINT NULL,
    Reason                   NVARCHAR(250) NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_InventoryAuditLog_IsSynthetic DEFAULT (1),
    ChangedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_InventoryAuditLog_ChangedUtc DEFAULT (SYSUTCDATETIME()),
    ChangedBy                NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_InventoryAuditLog PRIMARY KEY CLUSTERED (InventoryAuditLogId),
    CONSTRAINT FK_InventoryAuditLog_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId),
    CONSTRAINT CK_InventoryAuditLog_ChangeType_Valid CHECK (ChangeType IN (N'Sale', N'ManualAdjustment', N'Restock', N'PurchaseOrderCreated')),
    CONSTRAINT CK_InventoryAuditLog_QuantityBefore_NonNegative CHECK (QuantityBefore >= 0),
    CONSTRAINT CK_InventoryAuditLog_QuantityAfter_NonNegative CHECK (QuantityAfter >= 0)
);
GO

CREATE NONCLUSTERED INDEX IX_InventoryAuditLog_ProductId_ChangedUtc
    ON dbo.InventoryAuditLog(ProductId, ChangedUtc);

CREATE NONCLUSTERED INDEX IX_InventoryAuditLog_StoreCode_ChangedUtc
    ON dbo.InventoryAuditLog(StoreCode, ChangedUtc);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Tracks stock changes over time for traceability, debugging, and workflow demonstration.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'InventoryAuditLog';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for inventory audit entry.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'InventoryAuditLogId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the affected product.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ProductId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Store/location code where the stock change occurred.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'StoreCode';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of inventory movement such as Sale, ManualAdjustment, Restock, or PurchaseOrderCreated.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ChangeType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quantity on hand before the stock change.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'QuantityBefore';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Amount of stock added or removed; negative values are allowed for reductions.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'QuantityChanged';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quantity on hand after the stock change.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'QuantityAfter';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional reference entity type such as Order or PurchaseOrderDraft.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ReferenceType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional reference entity identifier.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ReferenceId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional reason for the stock adjustment.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'Reason';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the stock change happened.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ChangedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User, system, or process that made the stock change.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'InventoryAuditLog', @level2type=N'COLUMN', @level2name=N'ChangedBy';
GO

/*==================================================================================================
  AI DECISION
==================================================================================================*/
CREATE TABLE dbo.AiDecision
(
    AiDecisionId             BIGINT IDENTITY(1,1) NOT NULL,
    DecisionType             NVARCHAR(50) NOT NULL,
    EntityType               NVARCHAR(50) NOT NULL,
    EntityId                 BIGINT NOT NULL,
    ProductId                BIGINT NOT NULL,
    CorrelationId            UNIQUEIDENTIFIER NOT NULL,
    ModelName                NVARCHAR(100) NOT NULL,
    PromptVersion            NVARCHAR(30) NOT NULL,
    InputSummaryJson         NVARCHAR(MAX) NOT NULL,
    DecisionJson             NVARCHAR(MAX) NOT NULL,
    RecommendedAction        NVARCHAR(100) NOT NULL,
    ConfidenceScore          DECIMAL(5,2) NULL,
    RiskLevel                NVARCHAR(20) NULL,
    DecisionStatus           NVARCHAR(30) NOT NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_AiDecision_IsSynthetic DEFAULT (1),
    CreatedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_AiDecision_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    ApprovedUtc              DATETIME2(3) NULL,
    RejectedUtc              DATETIME2(3) NULL,
    ExecutedUtc              DATETIME2(3) NULL,

    CONSTRAINT PK_AiDecision PRIMARY KEY CLUSTERED (AiDecisionId),
    CONSTRAINT FK_AiDecision_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId),
    CONSTRAINT CK_AiDecision_DecisionType_Valid CHECK (DecisionType IN (N'ReplenishmentRecommendation')),
    CONSTRAINT CK_AiDecision_DecisionStatus_Valid CHECK (DecisionStatus IN (N'PendingApproval', N'Approved', N'Rejected', N'Executed', N'AutoApproved')),
    CONSTRAINT CK_AiDecision_RiskLevel_Valid CHECK (RiskLevel IS NULL OR RiskLevel IN (N'Low', N'Medium', N'High')),
    CONSTRAINT CK_AiDecision_ConfidenceScore_Range CHECK (ConfidenceScore IS NULL OR (ConfidenceScore >= 0 AND ConfidenceScore <= 100))
);
GO

CREATE NONCLUSTERED INDEX IX_AiDecision_ProductId_CreatedUtc
    ON dbo.AiDecision(ProductId, CreatedUtc);

CREATE NONCLUSTERED INDEX IX_AiDecision_CorrelationId
    ON dbo.AiDecision(CorrelationId);

CREATE NONCLUSTERED INDEX IX_AiDecision_DecisionStatus
    ON dbo.AiDecision(DecisionStatus);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores AI-generated replenishment recommendations, including structured inputs, outputs, and execution state.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'AiDecision';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for AI decision.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'AiDecisionId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of AI decision, currently ReplenishmentRecommendation.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'DecisionType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of business entity the decision is associated with.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'EntityType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identifier of the business entity the decision targets.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'EntityId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the product the replenishment decision is for.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'ProductId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correlation identifier used to trace the entire workflow.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'CorrelationId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Name of the AI model used for the decision.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'ModelName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Prompt version used when generating the decision.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'PromptVersion';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'JSON summary of the business context provided to the AI.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'InputSummaryJson';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'JSON payload of the structured AI decision.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'DecisionJson';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'High-level recommended action such as CreatePurchaseOrderDraft.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'RecommendedAction';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional confidence score from 0 to 100.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'ConfidenceScore';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional risk classification for the decision.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'RiskLevel';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Execution state of the AI decision.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'DecisionStatus';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the decision was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the decision was approved.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'ApprovedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the decision was rejected.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'RejectedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the decision was executed.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'AiDecision', @level2type=N'COLUMN', @level2name=N'ExecutedUtc';
GO

/*==================================================================================================
  APPROVAL REQUEST
==================================================================================================*/
CREATE TABLE dbo.ApprovalRequest
(
    ApprovalRequestId        BIGINT IDENTITY(1,1) NOT NULL,
    AiDecisionId             BIGINT NOT NULL,
    ApprovalType             NVARCHAR(50) NOT NULL,
    Status                   NVARCHAR(30) NOT NULL,
    RequestedBy              NVARCHAR(100) NOT NULL,
    RequestedUtc             DATETIME2(3) NOT NULL
        CONSTRAINT DF_ApprovalRequest_RequestedUtc DEFAULT (SYSUTCDATETIME()),
    ReviewedBy               NVARCHAR(100) NULL,
    ReviewedUtc              DATETIME2(3) NULL,
    ReviewComments           NVARCHAR(500) NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_ApprovalRequest_IsSynthetic DEFAULT (1),

    CONSTRAINT PK_ApprovalRequest PRIMARY KEY CLUSTERED (ApprovalRequestId),
    CONSTRAINT FK_ApprovalRequest_AiDecision_AiDecisionId
        FOREIGN KEY (AiDecisionId) REFERENCES dbo.AiDecision(AiDecisionId),
    CONSTRAINT UQ_ApprovalRequest_AiDecisionId UNIQUE (AiDecisionId),
    CONSTRAINT CK_ApprovalRequest_ApprovalType_Valid CHECK (ApprovalType IN (N'RestockApproval')),
    CONSTRAINT CK_ApprovalRequest_Status_Valid CHECK (Status IN (N'Pending', N'Approved', N'Rejected'))
);
GO

CREATE NONCLUSTERED INDEX IX_ApprovalRequest_Status
    ON dbo.ApprovalRequest(Status);

CREATE NONCLUSTERED INDEX IX_ApprovalRequest_RequestedUtc
    ON dbo.ApprovalRequest(RequestedUtc);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores human-in-the-loop approvals required for AI replenishment recommendations above configured thresholds.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'ApprovalRequest';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for approval request.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'ApprovalRequestId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the AI decision being reviewed.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'AiDecisionId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of approval request, currently RestockApproval.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'ApprovalType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current review status of the approval request.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'Status';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User or system that created the approval request.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'RequestedBy';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when approval was requested.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'RequestedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User who reviewed the request.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'ReviewedBy';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the request was reviewed.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'ReviewedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional review comments from the approver.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'ReviewComments';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'ApprovalRequest', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
GO

/*==================================================================================================
  PURCHASE ORDER DRAFT
==================================================================================================*/
CREATE TABLE dbo.PurchaseOrderDraft
(
    PurchaseOrderDraftId     BIGINT IDENTITY(1,1) NOT NULL,
    AiDecisionId             BIGINT NOT NULL,
    ProductId                BIGINT NOT NULL,
    StoreCode                NVARCHAR(30) NOT NULL,
    SuggestedQuantity        INT NOT NULL,
    EstimatedUnitCost        DECIMAL(18,2) NOT NULL,
    EstimatedTotalCost       DECIMAL(18,2) NOT NULL,
    DraftStatus              NVARCHAR(30) NOT NULL,
    Notes                    NVARCHAR(500) NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_PurchaseOrderDraft_IsSynthetic DEFAULT (1),
    CreatedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_PurchaseOrderDraft_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    ApprovedUtc              DATETIME2(3) NULL,

    CONSTRAINT PK_PurchaseOrderDraft PRIMARY KEY CLUSTERED (PurchaseOrderDraftId),
    CONSTRAINT FK_PurchaseOrderDraft_AiDecision_AiDecisionId
        FOREIGN KEY (AiDecisionId) REFERENCES dbo.AiDecision(AiDecisionId),
    CONSTRAINT FK_PurchaseOrderDraft_Product_ProductId
        FOREIGN KEY (ProductId) REFERENCES dbo.Product(ProductId),
    CONSTRAINT UQ_PurchaseOrderDraft_AiDecisionId UNIQUE (AiDecisionId),
    CONSTRAINT CK_PurchaseOrderDraft_SuggestedQuantity_Positive CHECK (SuggestedQuantity > 0),
    CONSTRAINT CK_PurchaseOrderDraft_EstimatedUnitCost_NonNegative CHECK (EstimatedUnitCost >= 0),
    CONSTRAINT CK_PurchaseOrderDraft_EstimatedTotalCost_NonNegative CHECK (EstimatedTotalCost >= 0),
    CONSTRAINT CK_PurchaseOrderDraft_DraftStatus_Valid CHECK (DraftStatus IN (N'Draft', N'Approved', N'Rejected', N'Submitted'))
);
GO

CREATE NONCLUSTERED INDEX IX_PurchaseOrderDraft_ProductId_CreatedUtc
    ON dbo.PurchaseOrderDraft(ProductId, CreatedUtc);

CREATE NONCLUSTERED INDEX IX_PurchaseOrderDraft_DraftStatus
    ON dbo.PurchaseOrderDraft(DraftStatus);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores the replenishment action proposed or created from an approved AI decision.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'PurchaseOrderDraft';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for purchase order draft.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'PurchaseOrderDraftId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the AI decision that led to this draft.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'AiDecisionId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Reference to the product being reordered.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'ProductId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Store/location code the replenishment is intended for.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'StoreCode';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Quantity recommended for replenishment.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'SuggestedQuantity';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estimated cost per unit at reorder time.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'EstimatedUnitCost';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estimated total reorder value.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'EstimatedTotalCost';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Current state of the purchase order draft.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'DraftStatus';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional business or AI notes related to the draft.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'Notes';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the draft was created.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the draft was approved, if applicable.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'PurchaseOrderDraft', @level2type=N'COLUMN', @level2name=N'ApprovedUtc';
GO

/*==================================================================================================
  SYSTEM EVENT
==================================================================================================*/
CREATE TABLE dbo.SystemEvent
(
    SystemEventId            BIGINT IDENTITY(1,1) NOT NULL,
    CorrelationId            UNIQUEIDENTIFIER NOT NULL,
    EventType                NVARCHAR(100) NOT NULL,
    EntityType               NVARCHAR(50) NOT NULL,
    EntityId                 BIGINT NULL,
    EventDataJson            NVARCHAR(MAX) NULL,
    ScenarioName             NVARCHAR(100) NULL,
    IsSynthetic              BIT NOT NULL
        CONSTRAINT DF_SystemEvent_IsSynthetic DEFAULT (1),
    CreatedUtc               DATETIME2(3) NOT NULL
        CONSTRAINT DF_SystemEvent_CreatedUtc DEFAULT (SYSUTCDATETIME()),
    CreatedBy                NVARCHAR(100) NOT NULL,

    CONSTRAINT PK_SystemEvent PRIMARY KEY CLUSTERED (SystemEventId)
);
GO

CREATE NONCLUSTERED INDEX IX_SystemEvent_CorrelationId_CreatedUtc
    ON dbo.SystemEvent(CorrelationId, CreatedUtc);

CREATE NONCLUSTERED INDEX IX_SystemEvent_EventType_CreatedUtc
    ON dbo.SystemEvent(EventType, CreatedUtc);
GO

EXEC sys.sp_addextendedproperty
    @name = N'MS_Description',
    @value = N'Stores workflow-level events for full traceability across orders, AI decisions, approvals, and execution steps.',
    @level0type = N'SCHEMA', @level0name = N'dbo',
    @level1type = N'TABLE',  @level1name = N'SystemEvent';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Primary key for workflow event.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'SystemEventId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Correlation identifier tying together one workflow execution path.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'CorrelationId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of event such as OrderPlaced, InventoryReduced, AiDecisionCreated, or ApprovalRequested.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'EventType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of business entity related to the event.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'EntityType';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional identifier of the related entity.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'EntityId';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional JSON payload describing the event details.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'EventDataJson';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Optional simulation scenario label.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'ScenarioName';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicates whether the row is synthetic/demo data.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'IsSynthetic';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'UTC timestamp when the event was recorded.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'CreatedUtc';
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User, process, or service that recorded the event.',
    @level0type=N'SCHEMA', @level0name=N'dbo', @level1type=N'TABLE', @level1name=N'SystemEvent', @level2type=N'COLUMN', @level2name=N'CreatedBy';
GO