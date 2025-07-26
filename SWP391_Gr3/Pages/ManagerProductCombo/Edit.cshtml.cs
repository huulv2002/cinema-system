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

            bool isDuplicate = await _context.Combos
                .AnyAsync(c => c.Title.ToLower() == ComboDto.Title.ToLower()
                    && c.IsActive == true
                    && c.Id != comboToUpdate.Id);

            if (isDuplicate)
            {
                ModelState.AddModelError("ComboDto.Title", "Tên combo đã tồn tại.");
            }
            if (SelectedProductIds == null || !SelectedProductIds.Any())
            {
                ModelState.AddModelError("", "Vui lòng chọn ít nhất một sản phẩm.");
            }

         
            foreach (var productId in SelectedProductIds)
            {
                var quantity = Quantities.ContainsKey(productId) ? Quantities[productId] : 1;
                var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

                if (product != null && quantity > product.Stock)
                {
                    ModelState.AddModelError("", $"Sản phẩm '{product.Name}' chỉ còn {product.Stock} trong kho.");
                }
            }

        
            if (!ModelState.IsValid)
            {
                AllProducts = await _context.Products.Where(p => p.IsActive).ToListAsync();
                return Page();
            }

           
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
