using Microsoft.AspNetCore.Mvc;
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
        public IList<Promotion> Promotions { get; set; } = new List<Promotion>();

        public async Task OnGetAsync()
        {
            Promotions = await _context.Promotions
                .Where(p => p.IsActive == true && (p.EndDate == null || p.EndDate >= DateTime.Today))
                .OrderByDescending(p => p.StartDate)
                .ToListAsync();
        }

    }
}
