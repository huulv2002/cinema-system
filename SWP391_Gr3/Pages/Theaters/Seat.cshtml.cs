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

        [BindProperty]
        public int SeatId { get; set; }

        public string RoomCode { get; set; }
        public List<Seat> Seats { get; set; }

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

            return Page();
        }

        public async Task<IActionResult> OnPostBookSeatAsync()
        {
            var seat = await _context.Seats.FindAsync(SeatId);
            if (seat == null || seat.IsActive == false)
            {
                return NotFound();
            }

            seat.IsActive = false;
            await _context.SaveChangesAsync();

            return RedirectToPage(new { RoomId = seat.RoomId });
        }
    }
}
