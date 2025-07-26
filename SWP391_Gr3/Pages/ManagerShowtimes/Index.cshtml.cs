using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManageShowtimes
{
    [AuthorizeRole("Owner")]
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;
        private const int PageSize = 15;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Showtime> Showtimes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            var now = DateTime.Now;
            var to = now.AddDays(7);

            var query = _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Room)
                .Where(s => s.StartTime >= now && s.StartTime <= to)
        .OrderBy(s => s.Movie != null ? s.Movie.Title.Length : 0); // 👉 Sắp theo độ dài tên phim

            int totalItems = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            Showtimes = await query
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
