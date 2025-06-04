using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Promotions
{
    public class DetailModel : PageModel
    {
        private readonly Swp391Context _context;
        public DetailModel(Swp391Context context) { _context = context; }

        public Promotion? Promotion { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Promotion = await _context.Promotions
                .Include(p => p.PromotionType)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Promotion == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
