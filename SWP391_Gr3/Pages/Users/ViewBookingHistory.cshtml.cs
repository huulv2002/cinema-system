using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class ViewBookingHistoryModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewBookingHistoryModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Ticket> UsedTickets { get; set; } = new();
        public List<Ticket> UnusedTickets { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            string? userIdStr = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
            {
                return RedirectToPage("/Users/Login");
            }

            var tickets = await _context.Tickets
                .Include(t => t.Seat)
                .Include(t => t.Showtime).ThenInclude(s => s.Movie)
                .Include(t => t.Order)
                .Where(t => t.Order != null && t.Order.UserId == userId)
                .ToListAsync();

            var now = DateTime.Now;

            UnusedTickets = tickets.Where(t => t.Showtime.StartTime > now).ToList();
            UsedTickets = tickets.Where(t => t.Showtime.StartTime <= now).ToList();

            return Page();
        }
    }
}
