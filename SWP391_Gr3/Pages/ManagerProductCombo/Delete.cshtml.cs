using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProductCombo
{
    public class DeleteModel : PageModel
    {
        private readonly Swp391Context _context;

        public DeleteModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Combo Combo { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Combo = await _context.Combos
                .Include(c => c.ProductCombos)
                    .ThenInclude(pc => pc.Product)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive == true);

            if (Combo == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int id)
        {
            var combo = await _context.Combos.FindAsync(id);

            if (combo == null)
            {
                return NotFound();
            }

            // Chuy?n tr?ng thái sang false
            combo.IsActive = false;

            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
