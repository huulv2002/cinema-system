using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerSeat
{
    [AuthorizeRole("Owner")]
    public class DeleteSeatModel : PageModel
    {


        private readonly Swp391Context _context;

        public DeleteSeatModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        [BindProperty(SupportsGet = true)]
        public int RoomId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int ShowtimeId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int MovieId { get; set; }

        public Seat? Seat { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Seat = await _context.Seats.FirstOrDefaultAsync(s => s.Id == Id && s.RoomId == RoomId);
            if (Seat == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var seat = await _context.Seats.FindAsync(Id);
            if (seat == null)
            {
                return NotFound();
            }

            try
            {
                _context.Seats.Remove(seat);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ErrorMessage = "Không thể xoá ghế vì đã có vé được đặt.";
                return RedirectToPage(new
                {
                    id = Id,
                    roomId = RoomId,
                    showtimeId = ShowtimeId,
                    movieId = MovieId
                });
            }

            return RedirectToPage("/ManagerSeat/SeatDetail", new
            {
                roomId = RoomId,
                showtimeId = ShowtimeId,
                movieId = MovieId
            });
        }
    }
}
