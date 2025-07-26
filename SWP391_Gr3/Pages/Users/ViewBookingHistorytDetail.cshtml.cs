using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Users
{
    [AuthorizeRole("Customer")]
    public class ViewBookingHistorytDetailModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewBookingHistorytDetailModel(Swp391Context context)
        {
            _context = context;
        }

        public OrderDetailViewModel? OrderDetail { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var order = await _context.Orders
                .Where(o => o.Id == id)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Seat)
                        .ThenInclude(s => s.Type)
                .Include(o => o.Tickets)
                    .ThenInclude(t => t.Showtime)
                        .ThenInclude(s => s.Movie)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Include(o => o.OrderCombos)
                    .ThenInclude(oc => oc.Combo)
                .FirstOrDefaultAsync();

            if (order == null) return NotFound();

            var firstShowtime = order.Tickets.FirstOrDefault()?.Showtime;

            OrderDetail = new OrderDetailViewModel
            {
                OrderId = order.Id,
                MovieTitle = firstShowtime?.Movie.Title ,
                Showtime = firstShowtime?.StartTime ?? DateTime.MinValue,
                Tickets = order.Tickets.Select(t => (
                    TicketCode: t.Code ,
                    SeatCode: t.Seat?.Code ,
                    SeatType: t.Seat?.Type?.Name ,
                    Price: t.Seat?.Type?.Price ?? 0
                    )).ToList(),


                Combos = order.OrderCombos.Select(oc => (
                    ComboName: oc.Combo.Title ,
                    Quantity: oc.Quantity ?? 0,
                    Price: oc.Combo?.Price ?? 0
                )).ToList(),
                Products = order.OrderProducts.Select(op => (
                    ProductName: op.Product.Name,
                     Quantity: op.Quantity ?? 0,
                     Price: op.Product?.Price ?? 0
                    )).ToList(),

                TotalPrice = CalculateTotal(order)
            };
                
            return Page();
        }

        private decimal CalculateTotal(Order order)
        {
            decimal ticketTotal = order.Tickets.Sum(t => t.Seat.Type.Price );
            decimal comboTotal = order.OrderCombos.Sum(c => (c.Combo?.Price ?? 0) * (c.Quantity ?? 1));
         
            return ticketTotal + comboTotal;
        }
    }
}
