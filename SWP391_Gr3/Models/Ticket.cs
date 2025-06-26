using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Ticket
{
    public int Id { get; set; }

    public int? ShowtimeId { get; set; }

    public string? Code { get; set; }

    public int? SeatId { get; set; }

    public int? OrderId { get; set; }

    public virtual Seat? Seat { get; set; }

    public virtual Showtime? Showtime { get; set; }
}
