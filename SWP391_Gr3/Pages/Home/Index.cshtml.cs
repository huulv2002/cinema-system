using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Home;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IMoviesService _moviesService;
    private readonly Swp391Context _context;

    public IndexModel(ILogger<IndexModel> logger, IMoviesService moviesService, Swp391Context context)
    {
        _logger = logger;
        _moviesService = moviesService;
        _context = context;
    }

    public List<Movie> Movies { get; set; } = new List<Movie>();
    public List<MovieReviewHomeViewModel> TopReviews { get; set; } = new();

    public async Task OnGetAsync()
    {
        Movies = (await _moviesService.ListAllMoviesAsync()).ToList();

        TopReviews = await _context.MovieReviews
            .OrderByDescending(r => r.PublishedDate)
            .ThenByDescending(r => r.Views)
            .Take(4)
            .Select(r => new MovieReviewHomeViewModel
            {
                Id = r.Id,
                Title = r.Title,
                ImageUrl = r.ImageUrl,
                Views = r.Views,
                Likes = r.Likes,
                Summary = r.Summary
            })
            .ToListAsync();
    }
}
