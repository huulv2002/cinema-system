using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Payment
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public string? Status { get; set; }

    public decimal? Amount { get; set; }
    public DateTime CreatedAt { get; set; } 

    public virtual Order? Order { get; set; }
}
