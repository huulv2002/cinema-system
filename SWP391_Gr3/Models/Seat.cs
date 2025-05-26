using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Seat
{
    public int Id { get; set; }

    public string? Position { get; set; }

    public int? TypeId { get; set; }

    public string? Code { get; set; }

    public int? RoomId { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual SeatType? Type { get; set; }
}
