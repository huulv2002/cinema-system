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
    public class IndexModel : PageModel
    {
        private readonly SWP391_Gr3.Models.Swp391Context _context;

        public IndexModel(SWP391_Gr3.Models.Swp391Context context)
        {
            _context = context;
        }

        public IList<MovieReview> MovieReview { get;set; } = default!;

        public async Task OnGetAsync()
        {
            MovieReview = await _context.MovieReviews
                .Include(m => m.Movie).ToListAsync();
        }
    }
}
