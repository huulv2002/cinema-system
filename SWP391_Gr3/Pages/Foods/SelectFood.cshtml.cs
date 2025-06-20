using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using SWP391_Gr3.ViewModels;
using SWP391_Gr3.Services;
using Microsoft.AspNetCore.Authorization;

namespace SWP391_Gr3.Pages.Foods
{
    [Authorize]
    public class SelectFoodModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly IEmailService _emailService;
        public SelectFoodModel(Swp391Context context, IEmailService emailService) { 
            _context = context;
            _emailService = emailService;
        }

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
            var userEmail = User.Identity?.Name;

            var showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(s => s.Id == ShowtimeId);

            string movieTitle = showtime?.Movie?.Title ?? "Kh�ng x�c ??nh";
            string showTimeStr = showtime?.StartTime != null
                ? showtime.StartTime.Value.ToString("HH:mm dd/MM/yyyy")
                : "Kh�ng x�c ??nh";

            var selectedFoods = await _context.Products
                .Where(p => SelectedFoodIds.Contains(p.Id))
                .Select(p => p.Name)
                .ToListAsync();

            var selectedCombos = await _context.Combos
                .Where(c => SelectedComboIds.Contains(c.Id))
                .Select(c => c.Title)
                .ToListAsync();

            var subject = "X�c nh?n ??t ?? ?n t?i r?p phim";
            var body = $"<b>B?n ?� ??t th�nh c�ng c�c m�n sau cho phim:</b><br/>" +
                       $"<b>Phim:</b> {movieTitle}<br/>" +
                       $"<b>Gi? chi?u:</b> {showTimeStr}<br/><br/>";
            if (selectedFoods.Any())
            {
                body += "<b>?? ?n ri�ng:</b><br/>- " + string.Join("<br/>- ", selectedFoods) + "<br/>";
            }
            if (selectedCombos.Any())
            {
                body += "<b>Combo:</b><br/>- " + string.Join("<br/>- ", selectedCombos) + "<br/>";
            }
            body += "<br/>C?m ?n b?n ?� s? d?ng d?ch v?!";

            if (!string.IsNullOrEmpty(userEmail))
            {
                await _emailService.SendEmailAsync(userEmail, subject, body);
            }

            // Chuy?n h??ng sang trang x�c nh?n ??t v�
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
