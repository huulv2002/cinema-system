using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ProductCategorys
{
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;

        public CreateModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = new();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            ProductCategory.IsActive = true;
            _context.ProductCategories.Add(ProductCategory);
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }

}
