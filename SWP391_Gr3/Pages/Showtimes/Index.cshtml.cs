using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Showtimes
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;
        public IndexModel(Swp391Context context) { _context = context; }

        public List<DateOnly> Dates { get; set; } = new();
        public IList<Theater> Theaters { get; set; } = new List<Theater>();
        public IList<Showtime> Showtimes { get; set; } = new List<Showtime>();
        public List<string> RoomTypes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public DateOnly? SelectedDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? TheaterId { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? RoomType { get; set; }
        [BindProperty(SupportsGet = true)]
        public int? MovieId { get; set; }

        public async Task OnGetAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            Dates = Enumerable.Range(0, 14).Select(i => today.AddDays(i)).ToList();

            if (!SelectedDate.HasValue)
                SelectedDate = today;

            Theaters = await _context.Rooms
                .Include(r => r.Theater)
                .Select(r => r.Theater)
                .Distinct()
                .ToListAsync();

            if (TheaterId.HasValue)
            {
                RoomTypes = await _context.Rooms
                    .Where(r => r.TheaterId == TheaterId.Value)
                    .Select(r => r.Code)
                    .Distinct()
                    .ToListAsync();
            }
            else
            {
                RoomTypes = await _context.Rooms
                    .Select(r => r.Code)
                    .Distinct()
                    .ToListAsync();
            }

            var query = _context.Showtimes
                .Include(s => s.Room).ThenInclude(r => r.Theater)
                .Include(s => s.Movie)
                .AsQueryable();

            if (MovieId.HasValue)
            {
                query = query.Where(s => s.MovieId == MovieId.Value);
            }
            if (SelectedDate.HasValue)
            {
                var date = SelectedDate.Value.ToDateTime(TimeOnly.MinValue).Date;
                query = query.Where(s => s.StartTime.HasValue && s.StartTime.Value.Date == date);
            }
            if (TheaterId.HasValue)
            {
                query = query.Where(s => s.Room != null && s.Room.TheaterId == TheaterId.Value);
            }
            if (!string.IsNullOrEmpty(RoomType))
            {
                query = query.Where(s => s.Room != null && s.Room.Code == RoomType);
            }

            Showtimes = await query.ToListAsync();
        }
    }
}
