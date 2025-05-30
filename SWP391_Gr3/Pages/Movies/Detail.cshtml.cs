using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Movies
{
    public class DetailModel : PageModel
    {
        private readonly Swp391Context _context;

        public DetailModel(Swp391Context context)
        {
            _context = context;
        }

        public Movie Movie { get; set; } = null!;
        public string? TrailerUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Images)
                .Include(m => m.Showtimes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            // Check if trailer file exists in wwwroot/trailers/{movie.Id}.mp4
            var trailerFile = Path.Combine("wwwroot", "trailers", $"{Movie.Id}.mp4");
            if (System.IO.File.Exists(trailerFile))
            {
                TrailerUrl = $"/trailers/{Movie.Id}.mp4";
            }
            else
            {
                TrailerUrl = null;
            }

            return Page();
        }
    }
}
