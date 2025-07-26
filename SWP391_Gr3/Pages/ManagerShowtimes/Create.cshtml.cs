using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManageShowtimes
{
    [AuthorizeRole("Owner")]
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;

        public CreateModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty] public int SelectedMovieId { get; set; }
        [BindProperty] public int SelectedRoomId { get; set; }
        [BindProperty] public DateTime StartDate { get; set; } = DateTime.Today;
        [BindProperty] public DateTime EndDate { get; set; } = DateTime.Today.AddDays(7);
        [BindProperty] public TimeSpan ShowTimeHour { get; set; }

        public List<Movie> Movies { get; set; } = new();
        public List<Room> Rooms { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Movies = await _context.Movies.OrderByDescending(m => m.Id).ToListAsync();
            Rooms = await _context.Rooms.ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (EndDate < StartDate)
            {
                ModelState.AddModelError(string.Empty, "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu.");
                await OnGetAsync();
                return Page();
            }

            var movie = await _context.Movies.FindAsync(SelectedMovieId);
            var room = await _context.Rooms.FindAsync(SelectedRoomId);

            if (movie == null || room == null)
            {
                ModelState.AddModelError(string.Empty, "Không tìm thấy phim hoặc phòng chiếu.");
                await OnGetAsync();
                return Page();
            }

            var currentDate = StartDate;
            while (currentDate <= EndDate)
            {
                var start = currentDate.Date + ShowTimeHour;
                var end = start.AddMinutes(movie.Duration ?? 0);

                var showtime = new Showtime
                {
                    MovieId = SelectedMovieId,
                    RoomId = SelectedRoomId,
                    StartTime = start,
                    EndTime = end,
                    IsMain = false
                };

                _context.Showtimes.Add(showtime);
                currentDate = currentDate.AddDays(1);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
}
