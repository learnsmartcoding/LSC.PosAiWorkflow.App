using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace LSC.PosAiWorkflow.Infrastructure.Persistence.Entities;

/// <summary>
/// Stores product categories used to group products for inventory and replenishment analysis.
/// </summary>
[Table("Category")]
[Index("Code", Name = "UQ_Category_Code", IsUnique = true)]
[Index("Name", Name = "UQ_Category_Name", IsUnique = true)]
public partial class Category
{
    /// <summary>
    /// Primary key for category.
    /// </summary>
    [Key]
    public long CategoryId { get; set; }

    /// <summary>
    /// Human-readable category name.
    /// </summary>
    [StringLength(100)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Short business code for the category.
    /// </summary>
    [StringLength(30)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// Optional category description.
    /// </summary>
    [StringLength(250)]
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the category is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Optional simulation scenario label such as HolidaySpike or NormalSales.
    /// </summary>
    [StringLength(100)]
    public string? ScenarioName { get; set; }

    /// <summary>
    /// Indicates whether the row was created from synthetic/demo data.
    /// </summary>
    public bool IsSynthetic { get; set; }

    /// <summary>
    /// UTC timestamp when the row was created.
    /// </summary>
    [Precision(3)]
    public DateTime CreatedUtc { get; set; }

    /// <summary>
    /// UTC timestamp when the row was last updated.
    /// </summary>
    [Precision(3)]
    public DateTime? UpdatedUtc { get; set; }

    [InverseProperty("Category")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
