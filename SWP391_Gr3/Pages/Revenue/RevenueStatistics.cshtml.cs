using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Revenue
{
    public class RevenueStatisticsModel : PageModel
    {
        private readonly Swp391Context _context;

        public RevenueStatisticsModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; } = "day";

        public List<string> RevenueLabels { get; set; } = new();
        public List<decimal> TotalRevenues { get; set; } = new();

        public decimal TotalRevenue => TotalRevenues.Sum();

        public void OnGet()
        {
            var tickets = _context.Tickets
                .Include(t => t.Order)
                .Include(t => t.Seat).ThenInclude(s => s.Type)
                .Where(t => t.Order != null && t.Seat != null && t.Seat.Type != null && t.Order.CreatedAt != null)
                .ToList();

            var orderProducts = _context.OrderProducts
                .Include(op => op.Order)
                .Include(op => op.Product)
                .Where(op => op.Order != null && op.Product != null && op.Order.CreatedAt != null)
                .ToList();

            var seatRevenueByGroup = tickets
                .Where(t => t.Order?.CreatedAt != null)
                .GroupBy(t => GetGroupKey(t.Order.CreatedAt))
                .Select(g => new { Key = g.Key, Revenue = g.Sum(t => t.Seat.Type.Price) })
                .ToList();

            var foodRevenueByGroup = orderProducts
                .Where(op => op.Order?.CreatedAt != null)
                .GroupBy(op => GetGroupKey(op.Order.CreatedAt))
                .Select(g => new { Key = g.Key, Revenue = g.Sum(op => op.Product.Price) })
                .ToList();


            var allKeys = seatRevenueByGroup.Select(x => x.Key)
                .Union(foodRevenueByGroup.Select(x => x.Key))
                .Distinct()
                .OrderBy(k => k)
                .ToList();

            RevenueLabels = allKeys;
            TotalRevenues = allKeys.Select(k =>
                (seatRevenueByGroup.FirstOrDefault(x => x.Key == k)?.Revenue ?? 0) +
                (foodRevenueByGroup.FirstOrDefault(x => x.Key == k)?.Revenue ?? 0)
            ).ToList();
        }

        private string GetGroupKey(DateTime? date)
        {
            if (!date.HasValue)
                return "Unknown";

            return Filter switch
            {
                "year" => date.Value.ToString("yyyy"),
                "month" => date.Value.ToString("yyyy-MM"),
                _ => date.Value.ToString("yyyy-MM-dd")
            };
        }
    }
}
