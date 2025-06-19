using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Foods
{
    public class SelectFoodModel : PageModel
    {
        private readonly Swp391Context _context;
        public SelectFoodModel(Swp391Context context) { _context = context; }

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SelectedSeatIds { get; set; } = "";

        public List<Product> FoodList { get; set; } = new();
        public List<ComboViewModel> ComboList { get; set; } = new();

        [BindProperty]
        public List<int> SelectedFoodIds { get; set; } = new();
        [BindProperty]
        public List<int> SelectedComboIds { get; set; } = new();

        public async Task OnGetAsync()
        {
            FoodList = await _context.Products.ToListAsync();

            var combos = await _context.Combos
    .Select(c => new ComboViewModel
    {
        Id = c.Id,
        Title = c.Title,
        Price = c.Price,
        Description = c.Description,
        Products = c.ProductCombos.Select(pc => new ComboProductItem
        {
            Name = pc.Product.Name,
            Quantity = pc.Quantity
        }).ToList()
    }).ToListAsync();

            ComboList = combos
                .GroupBy(c => new { c.Title, c.Price })
                .Select(g => g.First())
                .ToList();

        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("ConfirmBooking", new
            {
                ShowtimeId,
                SelectedSeatIds,
                FoodIds = string.Join(",", SelectedFoodIds),
                ComboIds = string.Join(",", SelectedComboIds)
            });
        }
    }
}
