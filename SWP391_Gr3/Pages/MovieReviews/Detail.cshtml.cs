using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.MovieReviews
{
    public class DetailModel : PageModel
    {
        private readonly Swp391Context _context;
        public DetailModel(Swp391Context context)
        {
            _context = context;
        }

        public MovieReview? MovieReview { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            MovieReview = await _context.MovieReviews
                .FirstOrDefaultAsync(r => r.Id == id);

            if (MovieReview == null)
            {
                return NotFound();
            }

            MovieReview.Views += 1;
            await _context.SaveChangesAsync();

            return Page();
        }
    }
}
