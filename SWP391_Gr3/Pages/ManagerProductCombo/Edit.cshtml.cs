using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Dtos;
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
        public ComboDto ComboDto { get; set; }

        public List<Product> AllProducts { get; set; } = new();

        [BindProperty]
        public List<int> SelectedProductIds { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> Quantities { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var combo = await _context.Combos
                .Include(c => c.ProductCombos)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (combo == null)
                return NotFound();

            ComboDto = new ComboDto
            {
                Id = combo.Id,
                Title = combo.Title,
                Price = combo.Price.Value,
                Description = combo.Description,


            };

            AllProducts = await _context.Products
                .Where(p => p.IsActive)
                .ToListAsync();

            SelectedProductIds = combo.ProductCombos.Select(pc => pc.ProductId ?? 0).ToList();
            Quantities = combo.ProductCombos
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

            // Validate: Không ch?n s?n ph?m
            if (SelectedProductIds == null || !SelectedProductIds.Any())
            {
                ModelState.AddModelError("", "Vui l?ng ch?n ít nh?t m?t s?n ph?m.");
            }

            // Validate s? lý?ng
            foreach (var productId in SelectedProductIds)
            {
                var quantity = Quantities.ContainsKey(productId) ? Quantities[productId] : 1;
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product != null && quantity > product.Stock)
                {
                    ModelState.AddModelError("", $"S?n ph?m '{product.Name}' ch? c?n {product.Stock} trong kho.");
                }
            }

            // ? Ki?m tra l?i SAU khi ð? validate custom
            if (!ModelState.IsValid)
            {
                AllProducts = await _context.Products.Where(p => p.IsActive).ToListAsync();
                return Page();
            }

            // ? C?p nh?t n?u h?p l?
            comboToUpdate.Title = ComboDto.Title;
            comboToUpdate.Price = ComboDto.Price;
            comboToUpdate.Description = ComboDto.Description;

            _context.ProductCombos.RemoveRange(comboToUpdate.ProductCombos);

            foreach (var productId in SelectedProductIds)
            {
                var quantity = Quantities.ContainsKey(productId) ? Quantities[productId] : 1;

                comboToUpdate.ProductCombos.Add(new ProductCombo
                {
                    ProductId = productId,
                    Quantity = quantity
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }

    }
}
