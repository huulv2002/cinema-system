using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class OrderCombo
{
    public int OrderId { get; set; }

    public int ComboId { get; set; }

    public int? Quantity { get; set; }

    public virtual Combo Combo { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
