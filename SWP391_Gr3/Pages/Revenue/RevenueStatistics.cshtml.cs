using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using System.Globalization;
using System.Linq;

namespace SWP391_Gr3.Pages.Revenue
{
    public class RevenueStatisticsModel : PageModel
    {
        private readonly Swp391Context _context;

        public decimal TotalSeatRevenue { get; set; }
        public decimal TotalFoodRevenue { get; set; }
        public decimal TotalRevenue => TotalSeatRevenue + TotalFoodRevenue;
        public List<string> DailyRevenueLabels { get; set; } = new();
        public List<decimal> DailySeatRevenue { get; set; } = new();
        public List<decimal> DailyFoodRevenue { get; set; } = new();
        public RevenueStatisticsModel(Swp391Context context)
        {
            _context = context;
        }

        public void OnGet()
        {
            var seatRevenueByDay = _context.Tickets
                .Where(t => t.OrderId != null && t.Order != null && t.Seat != null && t.Seat.Type != null)
                .GroupBy(t => t.Order.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(t => t.Seat.Type.Price) })
                .OrderBy(x => x.Date)
                .ToList();

            var foodRevenueByDay = _context.OrderProducts
                .Where(op => op.Product != null && op.Order != null)
                .GroupBy(op => op.Order.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Revenue = g.Sum(op => op.Product.Price) })
                .OrderBy(x => x.Date)
                .ToList();

            var allDates = seatRevenueByDay.Select(x => x.Date)
                .Union(foodRevenueByDay.Select(x => x.Date))
                .Distinct()
                .OrderBy(d => d)
                .ToList();

            DailyRevenueLabels = allDates.Select(d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)).ToList();
            DailySeatRevenue = allDates.Select(d => seatRevenueByDay.FirstOrDefault(x => x.Date == d)?.Revenue ?? 0).ToList();
            DailyFoodRevenue = allDates.Select(d => foodRevenueByDay.FirstOrDefault(x => x.Date == d)?.Revenue ?? 0).ToList();

            TotalSeatRevenue = seatRevenueByDay.Sum(x => x.Revenue);
            TotalFoodRevenue = foodRevenueByDay.Sum(x => x.Revenue);
        }
    }
}
