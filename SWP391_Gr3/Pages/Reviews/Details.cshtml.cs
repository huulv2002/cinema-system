using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Reviews
{
    [AuthorizeRole("Owner,Staff")]
    public class DetailsModel : PageModel
    {
        private readonly SWP391_Gr3.Models.Swp391Context _context;

        public DetailsModel(SWP391_Gr3.Models.Swp391Context context)
        {
            _context = context;
        }

        public MovieReview MovieReview { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            MovieReview = await _context.MovieReviews
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (MovieReview == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
