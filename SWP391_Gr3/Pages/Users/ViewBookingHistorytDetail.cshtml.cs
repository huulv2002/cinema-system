using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Users
{
    public class ViewBookingHistorytDetailModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewBookingHistorytDetailModel(Swp391Context context)
        {
            _context = context;
        }

        public Ticket Ticket { get; set; } = null!;
        public Promotion? Promotion { get; set; }
        public List<OrderCombo> OrderCombos { get; set; } = new();
        public List<ProductCombo> ProductCombos { get; set; } = new();
        public List<OrderProduct> OrderProducts { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Ticket = await _context.Tickets
                .Include(t => t.Seat).ThenInclude(s => s.Type)
                .Include(t => t.Showtime).ThenInclude(s => s.Movie)
                .Include(t => t.Order)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (Ticket == null || Ticket.Order == null)
            {
                return NotFound();
            }

            if (Ticket.Order.PromotionId != null)
            {
                Promotion = await _context.Promotions
                    .FirstOrDefaultAsync(p => p.Id == Ticket.Order.PromotionId);
            }

            OrderCombos = await _context.OrderCombos
                .Include(oc => oc.Combo)
                .Where(oc => oc.OrderId == Ticket.OrderId)
                .ToListAsync();

            ProductCombos = await _context.ProductCombos
                .Include(pc => pc.Product)
                .Where(pc => OrderCombos.Select(oc => oc.ComboId).Contains(pc.ComboId ?? 0))
                .ToListAsync();

            OrderProducts = await _context.OrderProducts
                .Include(op => op.Product)
                .Where(op => op.OrderId == Ticket.OrderId)
                .ToListAsync();

            return Page();
        }
    }
}
