using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Image
{
    public int Id { get; set; }

    public string? Url { get; set; }

    public int? MovieId { get; set; }

    public virtual Movie? Movie { get; set; }
}
