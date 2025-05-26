using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? TheaterId { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();

    public virtual Theater? Theater { get; set; }
}
