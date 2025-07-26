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

        public SelectFoodModel(Swp391Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }

        [BindProperty(SupportsGet = true, Name = "seatIds")]
        public string SelectedSeatIds { get; set; } = "";

        public List<Product> FoodList { get; set; } = new();
        public List<ComboViewModel> ComboList { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> FoodQuantities { get; set; } = new();

        [BindProperty]
        public List<int> SelectedComboIds { get; set; } = new();

        [BindProperty]
        public Dictionary<int, int> ComboQuantities { get; set; } = new();


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
                })
                .ToListAsync();

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

            // Parse seat ids từ chuỗi SelectedSeatIds
            var seatIds = SelectedSeatIds.Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(idStr => int.TryParse(idStr, out int id) ? id : -1)
                .Where(id => id > 0)
                .ToList();

            // Lấy giá vé tương ứng với từng Seat qua Ticket
            var seatTickets = await _context.Tickets
                .Include(t => t.Seat)
                    .ThenInclude(s => s.Type)
                .Where(t => t.ShowtimeId == ShowtimeId && t.SeatId != null && seatIds.Contains(t.SeatId.Value))
                .ToListAsync();

            decimal totalTicketPrice = seatTickets.Sum(t => t.Seat?.Type?.Price ?? 0);


            string movieTitle = showtime?.Movie?.Title ?? "Không xác định";
            string showTimeStr = showtime?.StartTime != null
                ? showtime.StartTime.Value.ToString("HH:mm dd/MM/yyyy")
                : "Không xác định";

            var selectedFoodIds = FoodQuantities
                .Where(kv => kv.Value > 0)
                .Select(kv => kv.Key)
                .ToList();

            var selectedFoods = await _context.Products
                .Where(p => selectedFoodIds.Contains(p.Id))
                .Select(p => new { p.Id, p.Name, p.Price })
                .ToListAsync();

            var selectedComboIds = ComboQuantities
                .Where(kv => kv.Value > 0)
                .Select(kv => kv.Key)
                .ToList();

            var selectedCombos = await _context.Combos
                .Where(c => selectedComboIds.Contains(c.Id))
                .Select(c => new { c.Id, c.Title, c.Price })
                .ToListAsync();


            decimal totalPrice = 0;
            foreach (var food in selectedFoods)
            {
                int quantity = FoodQuantities.ContainsKey(food.Id) ? FoodQuantities[food.Id] : 0;
                totalPrice += food.Price * quantity;
            }

            foreach (var combo in selectedCombos)
            {
                int quantity = ComboQuantities.ContainsKey(combo.Id) ? ComboQuantities[combo.Id] : 0;
                totalPrice += (combo.Price ?? 0) * quantity;
            }

            var subject = "Xác nhận đặt đồ ăn tại rạp phim";
            var body = $"<b>Bạn đã đặt thành công các món sau cho phim:</b><br/>" +
                       $"<b>Phim:</b> {movieTitle}<br/>" +
                       $"<b>Giờ chiếu:</b> {showTimeStr}<br/><br/>";

            if (selectedFoods.Any())
            {
                body += "<b>Đồ ăn riêng:</b><br/>";
                foreach (var food in selectedFoods)
                {
                    int quantity = FoodQuantities.ContainsKey(food.Id) ? FoodQuantities[food.Id] : 0;
                    body += $"- {food.Name} x {quantity} ({(food.Price * quantity):N0} đ)<br/>";
                }
            }

            if (selectedCombos.Any())
            {
                body += "<b>Combo:</b><br/>";
                foreach (var combo in selectedCombos)
                {
                    int quantity = ComboQuantities.ContainsKey(combo.Id) ? ComboQuantities[combo.Id] : 0;
                    body += $"- {combo.Title} x {quantity} ({(combo.Price ?? 0) * quantity:N0} đ)<br/>";
                }
            }

            body += $"<br/><b>Tổng giá vé:</b> {totalTicketPrice:N0} đ";
            body += $"<br/><b>Giá đồ ăn:</b> {totalPrice:N0} đ";
            body += $"<br/><b>Tổng cộng:</b> {(totalTicketPrice + totalPrice):N0} đ";
            body += "<br/><br/>Cảm ơn bạn đã sử dụng dịch vụ!";

            if (!string.IsNullOrEmpty(userEmail))
            {
                await _emailService.SendEmailAsync(userEmail, subject, body);
            }
            var foodIdQuantityPairs = FoodQuantities
                .Where(kv => kv.Value > 0)
                .Select(kv => $"{kv.Key}:{kv.Value}");

            string foodDataString = string.Join(",", foodIdQuantityPairs);

            var comboIdQuantityPairs = ComboQuantities
                .Where(kv => kv.Value > 0)
                .Select(kv => $"{kv.Key}:{kv.Value}");

            string comboDataString = string.Join(",", comboIdQuantityPairs);
            return RedirectToPage("ConfirmBooking", new
            {
                ShowtimeId,
                SelectedSeatIds,
                FoodData = foodDataString,
                ComboData = comboDataString
            });
        }
    }
}
