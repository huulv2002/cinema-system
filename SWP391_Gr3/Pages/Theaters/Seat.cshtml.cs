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
        public int SeatId { get; set; }

        public string RoomCode { get; set; } = "";
        public List<Seat> Seats { get; set; } = new();
        public List<int> BookedSeatIds { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var room = await _context.Rooms.Include(r => r.Seats).FirstOrDefaultAsync(r => r.Id == RoomId);
            if (room == null)
            {
                return NotFound();
            }

            RoomCode = room.Code;

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
            var seat = await _context.Seats.FindAsync(SeatId);
            if (seat == null)
                return NotFound();

            // kiểm tra xem ghế đã đặt cho suất chiếu này chưa
            bool alreadyBooked = await _context.Tickets
                .AnyAsync(t => t.SeatId == SeatId && t.ShowtimeId == ShowtimeId);
            if (alreadyBooked)
                return BadRequest("Ghế đã được đặt.");

            var ticket = new Ticket
            {
                SeatId = SeatId,
                ShowtimeId = ShowtimeId,
                Code = Guid.NewGuid().ToString().Substring(0, 8),
                OrderId = null // xử lý sau nếu cần
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { RoomId = RoomId, ShowtimeId = ShowtimeId });

        }
    }
}
