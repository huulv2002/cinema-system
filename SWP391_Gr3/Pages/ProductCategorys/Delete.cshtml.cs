using System.Runtime.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ProductCategorys
{
    [AuthorizeRole("Owner, Staff")]
    public class DeleteModel : PageModel
    {
        private readonly Swp391Context _context;

        public DeleteModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ProductCategory = await _context.ProductCategories.FindAsync(id);
            if (ProductCategory == null || !ProductCategory.IsActive)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var category = await _context.ProductCategories.FindAsync(ProductCategory.Id);
            var products = await _context.Products.Where(p => p.ProductCategory == category).ToListAsync();
            if (category != null)
            {
                category.IsActive = false; 
            }

            foreach(var p in products)
            {
                p.IsActive = false;

                var productCombos = await _context.ProductCombos.Where(c => c.Product == p).ToListAsync();

                foreach(var pc in productCombos)
                {
                    var combos = await _context.Combos.Where(c => c.ProductCombos.Contains(pc)).ToListAsync();
                    foreach(var c in combos)
                    {
                        c.IsActive = false;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }

}
