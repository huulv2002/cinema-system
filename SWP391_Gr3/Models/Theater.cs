using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Theater
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Location { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Combo> Combos { get; set; } = new List<Combo>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
