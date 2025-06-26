using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Size { get; set; }

    public decimal Price { get; set; }

    public int? Stock { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public int? ProductCategoryId { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual ProductCategory? ProductCategory { get; set; }

    public virtual ICollection<ProductCombo> ProductCombos { get; set; } = new List<ProductCombo>();
}
