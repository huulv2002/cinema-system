using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProductCombo
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Combo> Combos { get; set; }

        public async Task OnGetAsync()
        {
            Combos = await _context.Combos
                .Where(c => c.IsActive == true)
                .Include(c => c.ProductCombos)
                    .ThenInclude(pc => pc.Product)
                .ToListAsync();
        }
    }
}

