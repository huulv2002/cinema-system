using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProducts
{
    public class DeleteModel : PageModel
    {
        private readonly Swp391Context _context;

        public DeleteModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Product Product { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products
                .Include(p => p.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (Product == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var productInDb = await _context.Products.FindAsync(Product.Id);

            if (productInDb == null)
            {
                return NotFound();
            }
            productInDb.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
