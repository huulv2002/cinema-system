using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class PromotionType
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
