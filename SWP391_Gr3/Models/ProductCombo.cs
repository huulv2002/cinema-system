using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class ProductCombo
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? ComboId { get; set; }

    public virtual Combo? Combo { get; set; }

    public virtual Product? Product { get; set; }
}
