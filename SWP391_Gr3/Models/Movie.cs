using System;
using System.Collections.Generic;

namespace SWP391_Gr3.Models;

public partial class Movie
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public int? OverAge { get; set; }

    public string? Director { get; set; }

    public string? Performer { get; set; }

    public int? Duration { get; set; }

    public string? Language { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<MovieReview> MovieReviews { get; set; } = new List<MovieReview>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();
}
