using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
