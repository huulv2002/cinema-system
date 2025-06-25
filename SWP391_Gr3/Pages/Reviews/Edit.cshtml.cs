using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Reviews
{
    [Authorize(Roles = "Staff, Owner")]
    public class EditModel : PageModel
    {
        private readonly Swp391Context _context;

        public EditModel(Swp391Context context)
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

            var moviereview = await _context.MovieReviews.FindAsync(id);

            if (moviereview == null)
            {
                return NotFound();
            }

            MovieReview = moviereview;

            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", MovieReview.MovieId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", MovieReview.MovieId);

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is NOT valid:");
                foreach (var kvp in ModelState)
                {
                    foreach (var error in kvp.Value.Errors)
                    {
                        Console.WriteLine($"Error in {kvp.Key}: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            try
            {
                _context.Attach(MovieReview).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                Console.WriteLine("MovieReview updated successfully.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieReviewExists(MovieReview.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool MovieReviewExists(int id)
        {
            return _context.MovieReviews.Any(e => e.Id == id);
        }
    }
}
