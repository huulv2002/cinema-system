using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Reviews
{
    [Authorize(Roles = "Staff")]
    public class DeleteModel : PageModel
    {
        private readonly SWP391_Gr3.Models.Swp391Context _context;

        public DeleteModel(SWP391_Gr3.Models.Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public MovieReview MovieReview { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moviereview = await _context.MovieReviews.FirstOrDefaultAsync(m => m.Id == id);

            if (moviereview == null)
            {
                return NotFound();
            }
            else
            {
                MovieReview = moviereview;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var moviereview = await _context.MovieReviews.FindAsync(id);
            if (moviereview != null)
            {
                MovieReview = moviereview;
                _context.MovieReviews.Remove(MovieReview);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
