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
            // Lấy UserId từ session đăng nhập
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return; // Chưa đăng nhập
            }

            var now = DateTime.Now;
            var expireThreshold = now.AddMinutes(-30);

            Orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.Payment)
                .Include(o => o.Tickets).ThenInclude(t => t.Seat)
                .Include(o => o.Tickets).ThenInclude(t => t.Showtime).ThenInclude(s => s.Movie)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();

            // Gắn cờ hết hạn vào ViewData để sử dụng trong Razor
            foreach (var order in Orders)
            {
                if (order.Payment?.Status != "Success" && order.CreatedAt <= expireThreshold)
                {
                    ViewData[$"OrderExpired_{order.Id}"] = true;
                }
            }
        }
    }
}
