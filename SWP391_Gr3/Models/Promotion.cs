using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Promotion
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int PromotionTypeId { get; set; }

    public decimal? Value { get; set; }

    public bool IsActive { get; set; }

    public int? Stock { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? ImageUrl { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PromotionType? PromotionType { get; set; }
}
