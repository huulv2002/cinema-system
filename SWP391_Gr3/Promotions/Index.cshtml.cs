using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Promotions
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Promotion> Promotions { get; set; } = new();

        public async Task OnGetAsync()
        {
            var currentDate = DateTime.Now;
            Promotions = await _context.Promotions
                .Include(p => p.PromotionType)
                .Where(p => p.IsActive == true &&
                            p.StartDate <= currentDate &&
                            p.EndDate >= currentDate &&
                            (p.Stock == null || p.Stock > 0))
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();
        }
    }
}

