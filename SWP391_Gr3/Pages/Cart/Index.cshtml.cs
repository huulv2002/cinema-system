using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Order> Orders { get; set; } = new();

        public async Task OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return;
            }

            var now = DateTime.Now;
            var expireThreshold = now.AddMinutes(-30);

            Orders = await _context.Orders
                .Where(o => o.UserId == userId &&
                            (!o.IsConfirmed || o.Payment == null || o.Payment.Status.ToLower() != "success") &&
                            o.CreatedAt > expireThreshold)
                .Include(o => o.Payment)
                .Include(o => o.Tickets).ThenInclude(t => t.Seat)
                .Include(o => o.Tickets).ThenInclude(t => t.Showtime).ThenInclude(s => s.Movie)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }
    }
}
