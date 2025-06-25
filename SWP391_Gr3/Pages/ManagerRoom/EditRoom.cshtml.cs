using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerRoom
{
    [AuthorizeRole("Owner")]
    public class EditRoomModel : PageModel
    {
        private readonly Swp391Context _context;

        public EditRoomModel(Swp391Context context)
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
            var existingRoom = await _context.Rooms.FindAsync(Room.Id);
            if (existingRoom == null)
                return NotFound();

            existingRoom.Code = Room.Code;
            await _context.SaveChangesAsync();

            return RedirectToPage("/ManagerRoom/RoomManage", new
            {
                id = existingRoom.TheaterId,
                name = (await _context.Theaters.FindAsync(existingRoom.TheaterId))?.Name
            });
        }
    }
}
