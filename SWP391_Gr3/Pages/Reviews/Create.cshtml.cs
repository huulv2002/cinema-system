using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using Microsoft.Extensions.Logging;
namespace SWP391_Gr3.Pages.Reviews
{
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly ILogger<CreateModel> _logger;
        public CreateModel(Swp391Context context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public MovieReview MovieReview { get; set; } = new MovieReview();

        public SelectList MovieSelectList { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            MovieSelectList = new SelectList(movies, "Id", "Title");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        foreach (var error in state.Errors)
                        {
                            _logger.LogError($"ModelState Error - Field: {key}, Error: {error.ErrorMessage}");
                        }
                    }
                }

                var movies = await _context.Movies.ToListAsync();
                MovieSelectList = new SelectList(movies, "Id", "Title");
                return Page();
            }

            MovieReview.PublishedDate = DateTime.UtcNow;
            MovieReview.Likes = 0;
            MovieReview.Views = 0;

            _context.MovieReviews.Add(MovieReview);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
