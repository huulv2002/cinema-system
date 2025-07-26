using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Users
{
    [AuthorizeRole("Customer")]
    public class ViewBookingHistoryModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewBookingHistoryModel(Swp391Context context)
        {
            _context = context;
        }

        public List<OrderSummaryViewModel> UsedOrders { get; set; } = new();
        public List<OrderSummaryViewModel> UnusedOrders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToPage("/Users/Login");
            }

            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Seat)
                        .ThenInclude(seat => seat.Type)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Showtime)
                        .ThenInclude(s => s.Movie)
                .Include(o => o.OrderCombos)
                    .ThenInclude(oc => oc.Combo)
                .ToListAsync();

            var now = DateTime.Now;

            foreach (var order in orders)
            {
                var firstShowtime = order.Tickets.FirstOrDefault().Showtime;
                if (firstShowtime == null) continue;

                var summary = new OrderSummaryViewModel
                {
                    OrderId = order.Id,
                    MovieTitle = firstShowtime.Movie.Title,
                    ShowtimeDate = firstShowtime.StartTime.Value.Date,
                    TotalPrice = CalculateTotal(order)
                };

                if (firstShowtime.StartTime > now)
                    UnusedOrders.Add(summary);
                else
                    UsedOrders.Add(summary);
            }

            return Page();
        }

        private decimal CalculateTotal(Order order)
        {
            decimal ticketTotal = order.Tickets.Sum(t => t.Seat?.Type?.Price ?? 0);
            decimal comboTotal = order.OrderCombos.Sum(c => (c.Combo?.Price ?? 0) * (c.Quantity ?? 1));
            return ticketTotal + comboTotal;
        }
    }
}
