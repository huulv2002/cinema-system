using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Theaters
{
    public class RoomModel : PageModel
    {
        private readonly Swp391Context _context;

        public RoomModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public int TheaterId { get; set; }

        public string TheaterName { get; set; }
        public List<Room> Rooms { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var theater = await _context.Theaters.FirstOrDefaultAsync(t => t.Id == TheaterId);
            if (theater == null)
            {
                return NotFound();
            }

            TheaterName = theater.Name;

            Rooms = await _context.Rooms
                .Where(r => r.TheaterId == TheaterId)
                .Include(r => r.Seats) // để lấy số ghế
                .ToListAsync();

            return Page();
        }
    }
}
