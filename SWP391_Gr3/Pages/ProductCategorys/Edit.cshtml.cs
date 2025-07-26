using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ProductCategorys
{
    public class EditModel : PageModel
    {
        private readonly Swp391Context _context;

        public EditModel(Swp391Context context)
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
            if (!ModelState.IsValid)
                return Page();

            var existingCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(pc => pc.Id == ProductCategory.Id && pc.IsActive);

            if (existingCategory == null)
            {
                return NotFound();
            }

            var isExist = _context.ProductCategories.Where(pc => pc.Name == ProductCategory.Name).Any();
            if (isExist)
            {
                ModelState.AddModelError("ProductCategory.Name", "? Loai san pham nay da ton tai.");
                return Page();
            }
         
            existingCategory.Name = ProductCategory.Name;
            
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

    }

}
