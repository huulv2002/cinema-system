using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Revenue
{
    [AuthorizeRole("Owner")]
    public class RevenueStatisticsModel : PageModel
    {
        private readonly Swp391Context _context;

        public RevenueStatisticsModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public decimal TotalRevenue { get; set; }

        public class TopMovie
        {
            public string MovieName { get; set; } = string.Empty;
            public int TicketCount { get; set; }
        }

        public List<TopMovie> TopMovies { get; set; } = new();

        public void OnGet()
        {
            // ✅ Chỉ tính doanh thu khi có ngày bắt đầu và kết thúc
            if (StartDate.HasValue && EndDate.HasValue)
            {
                var tickets = _context.Tickets
                    .Include(t => t.Order)
                    .Include(t => t.Seat).ThenInclude(s => s.Type)
                    .Where(t => t.Order != null && t.Seat != null && t.Seat.Type != null && t.Order.CreatedAt != null)
                    .Where(t => t.Order.CreatedAt >= StartDate && t.Order.CreatedAt <= EndDate)
                    .ToList();

                var orderProducts = _context.OrderProducts
                    .Include(op => op.Order)
                    .Include(op => op.Product)
                    .Where(op => op.Order != null && op.Product != null && op.Order.CreatedAt != null)
                    .Where(op => op.Order.CreatedAt >= StartDate && op.Order.CreatedAt <= EndDate)
                    .ToList();

                var seatRevenue = tickets.Sum(t => t.Seat.Type.Price);
                var foodRevenue = orderProducts.Sum(op => op.Product.Price);
                TotalRevenue = seatRevenue + foodRevenue;
            }

            // ✅ Luôn tính Top 5 phim được đặt nhiều nhất (không theo khoảng thời gian)
            TopMovies = _context.Tickets
                .Include(t => t.Order)
                .Include(t => t.Showtime).ThenInclude(s => s.Movie)
                .Where(t =>
                    t.Order != null &&
                    t.Showtime != null &&
                    t.Showtime.Movie != null &&
                    t.Order.CreatedAt != null
                )
                .AsEnumerable()
                .GroupBy(t => t.Showtime.Movie.Title)
                .Select(g => new TopMovie
                {
                    MovieName = g.Key,
                    TicketCount = g.Count()
                })
                .OrderByDescending(m => m.TicketCount)
                .Take(5)
                .ToList();
        }
    }
}
