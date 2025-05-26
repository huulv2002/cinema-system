using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Combo
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public decimal? Price { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public int? TheaterId { get; set; }

    public virtual ICollection<OrderCombo> OrderCombos { get; set; } = new List<OrderCombo>();

    public virtual ICollection<ProductCombo> ProductCombos { get; set; } = new List<ProductCombo>();

    public virtual Theater? Theater { get; set; }
}
