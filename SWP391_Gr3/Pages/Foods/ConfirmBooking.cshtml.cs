using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public string SelectedSeatIds { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string? FoodIds { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? ComboIds { get; set; }

        public List<Seat> SelectedSeats { get; set; } = new();
        public List<Product> SelectedFoods { get; set; } = new();
        public List<Combo> SelectedCombos { get; set; } = new();

        public Movie Movie { get; set; }
        public Theater Theater { get; set; }
        public Room Room { get; set; }
        public Showtime Showtime { get; set; }

        public decimal TotalAmount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Showtime = await _context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Room).ThenInclude(r => r.Theater)
                .FirstOrDefaultAsync(s => s.Id == ShowtimeId);

            if (Showtime == null) return NotFound();

            Movie = Showtime.Movie;
            Room = Showtime.Room;
            Theater = Room.Theater;

            if (!string.IsNullOrEmpty(SelectedSeatIds))
            {
                var seatIdList = SelectedSeatIds.Split(',').Select(int.Parse).ToList();
                SelectedSeats = await _context.Seats.Include(s => s.Type)
                    .Where(s => seatIdList.Contains(s.Id)).ToListAsync();

                TotalAmount += SelectedSeats.Sum(s => s.Type.Price);
            }

            if (!string.IsNullOrEmpty(FoodIds))
            {
                var foodIdList = FoodIds.Split(',').Select(int.Parse).ToList();
                SelectedFoods = await _context.Products
                    .Where(p => foodIdList.Contains(p.Id)).ToListAsync();

                TotalAmount += SelectedFoods.Sum(p => p.Price);
            }

            if (!string.IsNullOrEmpty(ComboIds))
            {
                var comboIdList = ComboIds.Split(',').Select(int.Parse).ToList();
                SelectedCombos = await _context.Combos
                    .Where(c => comboIdList.Contains(c.Id)).ToListAsync();

                TotalAmount += SelectedCombos.Sum(c => c.Price ?? 0);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("UserId") == null)
                return RedirectToPage("/Users/Login");

            int userId = int.Parse(HttpContext.Session.GetString("UserId"));

            // Lấy lại dữ liệu như bên OnGet
            var seatIdList = SelectedSeatIds.Split(',').Select(int.Parse).ToList();
            var foodIdList = string.IsNullOrEmpty(FoodIds) ? new List<int>() : FoodIds.Split(',').Select(int.Parse).ToList();
            var comboIdList = string.IsNullOrEmpty(ComboIds) ? new List<int>() : ComboIds.Split(',').Select(int.Parse).ToList();

            var seats = await _context.Seats.Include(s => s.Type).Where(s => seatIdList.Contains(s.Id)).ToListAsync();
            var foods = await _context.Products.Where(p => foodIdList.Contains(p.Id)).ToListAsync();
            var combos = await _context.Combos.Where(c => comboIdList.Contains(c.Id)).ToListAsync();

            decimal totalAmount = seats.Sum(s => s.Type.Price) + foods.Sum(f => f.Price) + combos.Sum(c => c.Price ?? 0);

            // 1. Payment
            var payment = new Payment
            {
                Status = "Pending",
                Amount = totalAmount,
                CreatedAt = DateTime.Now
            };
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            // 2. Order
            var order = new Order
            {
                UserId = userId,
                PaymentId = payment.Id,
                CreatedAt = DateTime.Now,
                IsConfirmed = true
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 3. Ticket
            foreach (var seat in seats)
            {
                var ticket = new Ticket
                {
                    ShowtimeId = ShowtimeId,
                    SeatId = seat.Id,
                    OrderId = order.Id,
                    Code = Guid.NewGuid().ToString("N").Substring(0, 8)
                };
                _context.Tickets.Add(ticket);
            }

            // 4. OrderProduct
            foreach (var food in foods)
            {
                _context.OrderProducts.Add(new OrderProduct
                {
                    OrderId = order.Id,
                    ProductId = food.Id,
                    Quantity = 1
                });
            }

            // 5. OrderCombo
            foreach (var combo in combos)
            {
                _context.OrderCombos.Add(new OrderCombo
                {
                    OrderId = order.Id,
                    ComboId = combo.Id,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Cart/Index");
        }
    }
}
