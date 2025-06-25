using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerMovie
{
    [AuthorizeRole("Owner")]

    public class CreateMovieModel : PageModel
    {
        private readonly Swp391Context _context;
        public CreateMovieModel(Swp391Context context) => _context = context;

        [BindProperty]
        public Movie Movie { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
