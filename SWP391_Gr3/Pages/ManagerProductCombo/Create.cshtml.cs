using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProductCombo
{
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;

        public CreateModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Combo Combo { get; set; } = new();

        [BindProperty]
        public List<int> SelectedProductIds { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> Quantities { get; set; } = new();

        public List<Product> AllProducts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            AllProducts = await _context.Products
                .Where(p => p.IsActive)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid || SelectedProductIds == null || !SelectedProductIds.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một sản phẩm.");
                return Page();
            }

            Combo.IsActive = true;
            _context.Combos.Add(Combo);
            await _context.SaveChangesAsync();

            foreach (var productId in SelectedProductIds)
            {
                int quantity = Quantities.ContainsKey(productId) ? Quantities[productId] : 1;
                _context.ProductCombos.Add(new ProductCombo
                {
                    ComboId = Combo.Id,
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
