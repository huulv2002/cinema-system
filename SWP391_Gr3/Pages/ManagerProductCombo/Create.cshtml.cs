using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Dtos;
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
        public ComboDto Combo { get; set; } = new();

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
                if (SelectedProductIds == null || !SelectedProductIds.Any())
                {
                    ModelState.AddModelError("", "Vui lòng chọn ít nhất một sản phẩm.");
                }

                AllProducts = await _context.Products
                    .Where(p => p.IsActive)
                    .ToListAsync();

                return Page();
            }

            var selectedProducts = await _context.Products
                .Where(p => SelectedProductIds.Contains(p.Id))
                .ToListAsync();

            
            foreach (var product in selectedProducts)
            {
                int selectedQuantity = Quantities.ContainsKey(product.Id) ? Quantities[product.Id] : 1;
                if (selectedQuantity > product.Stock)
                {
                    ModelState.AddModelError("", $"Sản phẩm '{product.Name}' chỉ còn {product.Stock} trong kho.");
                }
            }

            if (!ModelState.IsValid)
            {
                AllProducts = await _context.Products
                    .Where(p => p.IsActive)
                    .ToListAsync();

                return Page();
            }
          
            bool isDuplicate = await _context.Combos
                .AnyAsync(c => c.Title.ToLower() == Combo.Title.ToLower() && c.IsActive == true);

            if (isDuplicate)
            {
                ModelState.AddModelError("Combo.Title", "Tên combo đã tồn tại.");
                AllProducts = await _context.Products
                    .Where(p => p.IsActive)
                    .ToListAsync();
                return Page();
            }


            var newCombo = new Combo
            {
                Title = Combo.Title,
                Price = Combo.Price,
                Description = Combo.Description,
                IsActive = Combo.IsActive,
                TheaterId = Combo.TheaterId
            };

            _context.Combos.Add(newCombo);
            await _context.SaveChangesAsync();

            foreach (var product in selectedProducts)
            {
                int quantity = Quantities.ContainsKey(product.Id) ? Quantities[product.Id] : 1;
                _context.ProductCombos.Add(new ProductCombo
                {
                    ComboId = newCombo.Id,
                    ProductId = product.Id,
                    Quantity = quantity
                });
            }
    
            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
    }
