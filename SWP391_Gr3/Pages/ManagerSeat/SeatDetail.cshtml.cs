using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerSeat
{
    [AuthorizeRole("Owner")]

    public class SeatDetailModel : PageModel
    {
        private readonly Swp391Context _context;
        public SeatDetailModel(Swp391Context context) => _context = context;

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public string RoomCode { get; set; } = "";
        public List<Seat> Seats { get; set; } = new();
        public List<SeatType> SeatTypes { get; set; } = new();
        public Movie? Movie { get; set; }
        public Showtime? Showtime { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var room = await _context.Rooms.FindAsync(RoomId);
            if (room == null) return NotFound();

            RoomCode = room.Code;

            Seats = await _context.Seats
                .Where(s => s.RoomId == RoomId)
                .Include(s => s.Type)
                .ToListAsync();

            SeatTypes = await _context.SeatTypes.ToListAsync();
            Movie = await _context.Movies.FindAsync(MovieId);
            Showtime = await _context.Showtimes.FindAsync(ShowtimeId);

            return Page();
        }


        public async Task<IActionResult> OnPostAddAsync(string NewSeatCode, int NewSeatTypeId)
        {
            var seat = new Seat
            {
                Code = NewSeatCode,
                TypeId = NewSeatTypeId,
                RoomId = RoomId
            };
            _context.Seats.Add(seat);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { showtimeId = ShowtimeId, roomId = RoomId, movieId = MovieId });
        }

        public async Task<IActionResult> OnPostEditAsync(int EditSeatId, string EditSeatCode, int EditSeatTypeId)
        {
            var seat = await _context.Seats.FindAsync(EditSeatId);
            if (seat != null)
            {
                seat.Code = EditSeatCode;
                seat.TypeId = EditSeatTypeId;
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { showtimeId = ShowtimeId, roomId = RoomId, movieId = MovieId });
        }
    }
}
