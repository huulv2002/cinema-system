using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Theaters
{
    public class CreateRoomModel : PageModel
    {
        private readonly ITheatersService _theatersService;

        public CreateRoomModel(ITheatersService theatersService)
        {
            _theatersService = theatersService;
        }
        [BindProperty]
        public Room Room { get; set; } = new Room();
        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            // Check for duplicate Room.Code
            bool roomExists = await _theatersService.RoomCodeExistsAsync(Room.Code, id);
            if (roomExists)
            {
                ModelState.AddModelError("Room.Code", "Mã phòng đã tồn tại trong rạp này. Vui lòng chọn mã khác.");
                return Page();
            }
            var result = await _theatersService.CreateRoomAsync(Room,id);
            if (result)
            {
                return RedirectToPage("/Theaters/RoomManage", new { id });
            }

            ModelState.AddModelError(string.Empty, "Không thể phòng. Vui lòng thử lại.");
            return Page();
        }
    }
}
