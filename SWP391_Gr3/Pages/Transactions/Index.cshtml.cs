using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Transactions
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;
        public IndexModel(Swp391Context context) { _context = context; }

        public IList<Transaction> Transactions { get; set; } = new List<Transaction>();
        public List<string> StatusList { get; set; } = new();
        public List<Showtime> Showtimes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Status { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? Date { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? ShowtimeId { get; set; }

        public async Task OnGetAsync()
        {
            int theaterId = 1; 
            StatusList = await _context.Transactions
                .Select(t => t.Status)
                .Distinct()
                .ToListAsync();

            Showtimes = await _context.Showtimes
                .Where(s => s.Room.TheaterId == theaterId)
                .OrderByDescending(s => s.StartTime)
                .ToListAsync();

            var query = _context.Transactions
                .Include(t => t.User)
                .Include(t => t.Seat)
                .Include(t => t.Showtime)
                    .ThenInclude(s => s.Room)
                .Where(t => t.Showtime.Room.TheaterId == theaterId)
                .AsQueryable();

            if (!string.IsNullOrEmpty(Status))
            {
                query = query.Where(t => t.Status == Status);
            }
            if (Date.HasValue)
            {
                var date = Date.Value.Date;
                query = query.Where(t => t.CreatedAt.Date == date);
            }
            if (ShowtimeId.HasValue)
            {
                query = query.Where(t => t.ShowtimeId == ShowtimeId.Value);
            }

            Transactions = await query
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
