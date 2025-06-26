using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerRoom
{
    [AuthorizeRole("Owner")]
    public class DetailRoomModel : PageModel
    {
        private readonly Swp391Context _context;

        public DetailRoomModel(Swp391Context context)
        {
            _context = context;
        }

        public Room? Room { get; set; }

        public List<DateOnly> Dates { get; set; } = new();
        public List<Showtime> FilteredShowtimes { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public DateOnly? SelectedDate { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Room = await _context.Rooms
                .Include(r => r.Theater)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Room == null)
                return NotFound();

            var today = DateOnly.FromDateTime(DateTime.Today);
            Dates = Enumerable.Range(0, 14).Select(i => today.AddDays(i)).ToList();

            SelectedDate ??= today;

            FilteredShowtimes = await _context.Showtimes
                .Include(s => s.Movie)
                .Where(s =>
                    s.RoomId == id &&
                    s.StartTime.HasValue &&
                    DateOnly.FromDateTime(s.StartTime.Value) == SelectedDate)
                .ToListAsync();

            return Page();
        }
    }
}
