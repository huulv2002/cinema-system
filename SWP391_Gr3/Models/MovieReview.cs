using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class MovieReview
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Summary { get; set; }

    public string Content { get; set; } = null!;

    public string? ImageUrl { get; set; }

    public DateTime PublishedDate { get; set; }

    public int Likes { get; set; }

    public int Views { get; set; }

    public int MovieId { get; set; }

    public virtual Movie Movie { get; set; } = null!;
}
