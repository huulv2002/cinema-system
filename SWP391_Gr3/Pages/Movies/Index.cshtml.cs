using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public IList<Movie> Movies { get; set; } = new List<Movie>();
        public IList<Genre> Genres { get; set; } = new List<Genre>();

        [BindProperty(SupportsGet = true)]
        public int? GenreId { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateOnly? MinReleaseDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? MaxReleaseDate { get; set; }

        public async Task OnGetAsync()
        {
            Genres = await _context.Genres.ToListAsync();

            var query = _context.Movies
                .Include(m => m.Genres)
                .Include(m => m.Images)
                .Include(m => m.Showtimes)
                .AsQueryable();

            if (GenreId.HasValue && GenreId.Value > 0)
            {
                query = query.Where(m => m.Genres.Any(g => g.Id == GenreId.Value));
            }

            if (MinReleaseDate.HasValue)
            {
                query = query.Where(m => m.ReleaseDate >= MinReleaseDate.Value);
            }
            if (MaxReleaseDate.HasValue)
            {
                query = query.Where(m => m.ReleaseDate <= MaxReleaseDate.Value);
            }

            Movies = await query.ToListAsync();
        }
    }
}
