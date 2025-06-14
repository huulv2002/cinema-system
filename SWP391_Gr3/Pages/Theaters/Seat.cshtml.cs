using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Theaters
{
    public class SeatModel : PageModel
    {
        private readonly Swp391Context _context;

        public SeatModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }

        [BindProperty]
        public string SelectedSeatIds { get; set; }

        public string RoomCode { get; set; } = "";
        public List<Seat> Seats { get; set; } = new();
        public List<int> BookedSeatIds { get; set; } = new();

        public Movie Movie { get; set; }
        public Room Room { get; set; }
        public Theater Theater { get; set; }
        public Showtime Showtime { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Showtime = await _context.Showtimes
                .Include(s => s.Movie).ThenInclude(m => m.Images)
                .Include(s => s.Room).ThenInclude(r => r.Theater)
                .FirstOrDefaultAsync(s => s.Id == ShowtimeId);

            if (Showtime == null) return NotFound();

            Movie = Showtime.Movie;
            Room = Showtime.Room;
            Theater = Room.Theater;
            RoomId = Room.Id;
            RoomCode = Room.Code;

            Seats = await _context.Seats
                .Where(s => s.RoomId == RoomId)
                .Include(s => s.Type)
                .ToListAsync();

            BookedSeatIds = await _context.Tickets
                .Where(t => t.ShowtimeId == ShowtimeId && t.SeatId != null)
                .Select(t => t.SeatId.Value)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostBookSeatAsync()
        {
            if (string.IsNullOrEmpty(SelectedSeatIds))
            {
                ModelState.AddModelError("", "Bạn chưa chọn ghế nào.");
                return await OnGetAsync();
            }

            var seatIds = SelectedSeatIds
                .Split(',', System.StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id, out var sId) ? sId : -1)
                .Where(id => id > 0)
                .Distinct()
                .ToList();

            if (seatIds.Count > 5)
            {
                ModelState.AddModelError("", "Chỉ được chọn tối đa 5 ghế.");
                return await OnGetAsync();
            }

            foreach (var seatId in seatIds)
            {
                bool alreadyBooked = await _context.Tickets
                    .AnyAsync(t => t.SeatId == seatId && t.ShowtimeId == ShowtimeId);

                if (alreadyBooked) continue;

                var ticket = new Ticket
                {
                    SeatId = seatId,
                    ShowtimeId = ShowtimeId,
                    Code = Guid.NewGuid().ToString("N")[..8],
                    OrderId = null
                };

                _context.Tickets.Add(ticket);
            }

            await _context.SaveChangesAsync();

            return RedirectToPage(new { RoomId = RoomId, ShowtimeId = ShowtimeId });
        }
    }
}
