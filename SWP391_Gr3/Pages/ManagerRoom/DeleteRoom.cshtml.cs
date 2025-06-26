using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerRoom
{
    [AuthorizeRole("Owner")]
    public class DeleteRoomModel : PageModel
    {
        private readonly Swp391Context _context;

        public DeleteRoomModel(Swp391Context context)
        {
            _context = context;
        }

        [BindProperty]
        public Room Room { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Room = await _context.Rooms
                .Include(r => r.Theater)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (Room == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roomToDelete = await _context.Rooms
                .Include(r => r.Theater)
                .FirstOrDefaultAsync(r => r.Id == Room.Id);

            if (roomToDelete == null)
                return NotFound();

            int theaterId = (int)roomToDelete.TheaterId;
            string theaterName = roomToDelete.Theater?.Name ?? "";

            _context.Rooms.Remove(roomToDelete);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ManagerRoom/RoomManage", new
            {
                id = theaterId,
                name = theaterName
            });
        }
    }
}
