using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProductCombo
{
    public class EditModel : PageModel
    {
        private readonly Swp391Context _context;

        public EditModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Combo Combo { get; set; }

        public List<Product> AllProducts { get; set; } = new();

        [BindProperty]
        public List<int> SelectedProductIds { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> Quantities { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Combo = await _context.Combos
                .Include(c => c.ProductCombos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (Combo == null)
                return NotFound();

            AllProducts = await _context.Products
                .Where(p => p.IsActive)
                .ToListAsync();

            SelectedProductIds = Combo.ProductCombos.Select(pc => pc.ProductId ?? 0).ToList();
            Quantities = Combo.ProductCombos
                .Where(pc => pc.ProductId.HasValue && pc.Quantity.HasValue)
                .ToDictionary(pc => pc.ProductId!.Value, pc => pc.Quantity!.Value);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var comboToUpdate = await _context.Combos
                .Include(c => c.ProductCombos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comboToUpdate == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                AllProducts = await _context.Products.Where(p => p.IsActive).ToListAsync();
                return Page();
            }

            comboToUpdate.Title = Combo.Title;
            comboToUpdate.Price = Combo.Price;
            comboToUpdate.Description = Combo.Description;

            // Xóa các productcombo c?
            _context.ProductCombos.RemoveRange(comboToUpdate.ProductCombos);

            // Thêm l?i các productcombo m?i
            if (SelectedProductIds != null)
            {
                foreach (var productId in SelectedProductIds)
                {
                    var quantity = Quantities.ContainsKey(productId) ? Quantities[productId] : 1;

                    comboToUpdate.ProductCombos.Add(new ProductCombo
                    {
                        ProductId = productId,
                        Quantity = quantity
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}