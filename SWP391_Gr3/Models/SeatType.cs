using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class SeatType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
