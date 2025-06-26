using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWP391_Gr3.Models;

public partial class PromotionType
{
    public int Id { get; set; }
    [Required]
    public string? Title { get; set; }

    public virtual ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
}
