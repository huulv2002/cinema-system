using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.MovieReviews
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;
        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<MovieReviewHomeViewModel> Reviews { get; set; } = new();

        public async Task OnGetAsync()
        {
            Reviews = await _context.MovieReviews
                .OrderByDescending(r => r.PublishedDate)
                .Select(r => new MovieReviewHomeViewModel
                {
                    Id = r.Id,
                    Title = r.Title,
                    ImageUrl = r.ImageUrl,
                    Views = r.Views,
                    Likes = r.Likes,
                    Summary = r.Summary
                })
                .ToListAsync();
        }
    }
}
