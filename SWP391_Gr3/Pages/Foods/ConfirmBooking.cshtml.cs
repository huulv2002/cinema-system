using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System.Security.Claims;

namespace SWP391_Gr3.Pages.Foods
{
    public class ConfirmBookingModel : PageModel
    {
        private readonly Swp391Context _context;

        public ConfirmBookingModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedSeatIds { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string ComboIds { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string FoodData { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string ComboData { get; set; } = string.Empty;

        public Dictionary<int, int> ComboQuantities { get; set; } = new();

        public Showtime Showtime { get; set; }
        public Movie Movie { get; set; }
        public Room Room { get; set; }
        public Theater Theater { get; set; }

        public List<Seat> SelectedSeats { get; set; } = new();
        public List<Product> SelectedFoods { get; set; } = new();
        public List<Combo> SelectedCombos { get; set; } = new();

        public Dictionary<int, int> FoodQuantities { get; set; } = new();
        public decimal TotalAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadBookingDataAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadBookingDataAsync();

            // Get user ID from claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null) return Unauthorized();

            int userId = int.Parse(userIdClaim.Value);

            // 1. Create Payment
            var payment = new Payment
            {
                Status = "Pending",
                Amount = TotalAmount,
                CreatedAt = DateTime.Now
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // 2. Create Order
            var order = new Order
            {
                UserId = userId,
                PaymentId = payment.Id,
                CreatedAt = DateTime.Now,
                IsConfirmed = true
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 3. Create Tickets
            foreach (var seat in SelectedSeats)
            {
                _context.Tickets.Add(new Ticket
                {
                    ShowtimeId = ShowtimeId,
                    SeatId = seat.Id,
                    OrderId = order.Id,
                    Code = Guid.NewGuid().ToString("N").Substring(0, 8)
                });
            }

            // 4. Create OrderProduct with quantity
            foreach (var food in SelectedFoods)
            {
                _context.OrderProducts.Add(new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = food.Id,
                    Quantity = FoodQuantities[food.Id]
                });
            }

            // 5. Create OrderCombos (default quantity = 1 per combo)
            foreach (var combo in SelectedCombos)
            {
                _context.OrderCombos.Add(new OrderCombo
                {
                    OrderId = order.Id,
                    ComboId = combo.Id,
                    Quantity = ComboQuantities[combo.Id]
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/Cart/Index");
        }

        private async Task LoadBookingDataAsync()
        {
            Showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Room).ThenInclude(r => r.Theater)
                .FirstOrDefaultAsync(s => s.Id == ShowtimeId)
                ?? throw new Exception("Showtime not found");

            Movie = Showtime.Movie!;
            Room = Showtime.Room!;
            Theater = Room.Theater!;

            var seatIds = SelectedSeatIds.Split(',').Where(id => int.TryParse(id, out _)).Select(int.Parse).ToList();
            SelectedSeats = await _context.Seats
                .Include(s => s.Type)
                .Where(s => seatIds.Contains(s.Id))
                .ToListAsync();

            if (!string.IsNullOrEmpty(FoodData))
            {
                FoodQuantities = FoodData
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(pair => pair.Split(':'))
                    .Where(parts => parts.Length == 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _))
                    .ToDictionary(pair => int.Parse(pair[0]), pair => int.Parse(pair[1]));
            }

            var foodIds = FoodQuantities.Keys.ToList();
            SelectedFoods = await _context.Products
                .Where(p => foodIds.Contains(p.Id))
                .ToListAsync();

            if (!string.IsNullOrEmpty(ComboData))
            {
                ComboQuantities = ComboData
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(pair => pair.Split(':'))
                    .Where(parts => parts.Length == 2 && int.TryParse(parts[0], out _) && int.TryParse(parts[1], out _))
                    .ToDictionary(pair => int.Parse(pair[0]), pair => int.Parse(pair[1]));
            }

            var comboIds = ComboQuantities.Keys.ToList();
            SelectedCombos = await _context.Combos
                .Where(c => comboIds.Contains(c.Id))
                .ToListAsync();

            TotalAmount = SelectedSeats.Sum(s => s.Type.Price)
             + SelectedFoods.Sum(f => f.Price * FoodQuantities[f.Id])
             + SelectedCombos.Sum(c => (c.Price ?? 0) * ComboQuantities[c.Id]);
        }
    }
}
