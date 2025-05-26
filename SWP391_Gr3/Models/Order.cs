using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? PaymentId { get; set; }

    public int? PromotionId { get; set; }

    public virtual ICollection<OrderCombo> OrderCombos { get; set; } = new List<OrderCombo>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Payment? Payment { get; set; }

    public virtual Promotion? Promotion { get; set; }

    public virtual User? User { get; set; }
}
