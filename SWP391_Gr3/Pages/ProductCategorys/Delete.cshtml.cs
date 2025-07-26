using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ProductCategorys
{
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
            if (category != null)
            {
                category.IsActive = false; 
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }

}
