using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerMovie
{
    [AuthorizeRole("Owner")]

    public class DeleteMovieModel : PageModel
    {
        private readonly Swp391Context _context;

        public DeleteMovieModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public Movie? Movie { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        // ✅ Đây là hàm GET để hiển thị trang xác nhận
        public async Task<IActionResult> OnGetAsync()
        {
            Movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == Id);
            if (Movie == null)
                return NotFound();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var movie = await _context.Movies
                .Include(m => m.Images)
                .Include(m => m.Showtimes)
                    .ThenInclude(st => st.Tickets)
                .FirstOrDefaultAsync(m => m.Id == Id);

            if (movie == null)
                return NotFound();

            if (movie.Showtimes.Any(st => st.Tickets.Any()))
            {
                ErrorMessage = "Không thể xoá phim vì đã có vé được đặt.";
                return RedirectToPage(new { Id });
            }

            if (movie.Images.Any())
            {
                _context.Images.RemoveRange(movie.Images);
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ManagerMovie/Index");
        }
    }
}
