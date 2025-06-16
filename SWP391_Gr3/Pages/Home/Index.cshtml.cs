using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Home;

public class IndexModel : PageModel
{

    private readonly ILogger<IndexModel> _logger;
    private readonly IMoviesService _moviesService;
    public IndexModel(ILogger<IndexModel> logger, IMoviesService moviesService)
    {
        _logger = logger;
        _moviesService = moviesService;
    }
    public List<Movie> Movies { get; set; } = new List<Movie>();
    public async Task OnGetAsync()
    {
        Movies = (await _moviesService.ListAllMoviesAsync()).ToList();
    }
}
