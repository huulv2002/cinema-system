using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Transaction
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ShowtimeId { get; set; }

    public int SeatId { get; set; }

    public decimal Amount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Seat Seat { get; set; } = null!;

    public virtual Showtime Showtime { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
