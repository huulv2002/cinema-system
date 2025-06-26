using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    [AuthorizeRole("Owner,Admin")]
    public class AsignsModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly ITheatersService _theatersService;
        public AsignsModel(IUserService userService, ITheatersService theatersService)
        {
            _userService = userService;
            _theatersService = theatersService;
        }
        [BindProperty]
        public int UserId { get; set; }

        [BindProperty]
        public int TheaterId { get; set; }

        public User User { get; set; }
        public System.Collections.Generic.IEnumerable<Theater> Theaters { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var Users = await _userService.GetUserByIdAsync(id);
            if (Users == null)
            {
                TempData["Message"] = "Không tìm thấy nhân viên0.";
                return Page();
            }

            UserId = id;
            User = Users;
            Theaters = await _theatersService.ListAllTheaterAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var User = await _userService.GetUserByIdAsync(UserId);
                Theaters = await _theatersService.ListAllTheaterAsync();
                return Page();
            }

            var user = await _userService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                TempData["Message"] = "Không tìm thấy nhân viên1.";
                return Page();
            }

            // Kiểm tra xem TheaterId có hợp lệ
            var theater = await _theatersService.ListAllTheaterAsync();
            if (!theater.Any(t => t.Id == TheaterId))
            {
                ModelState.AddModelError("TheaterId", "Rạp phim không tồn tại.");
                User = user;
                Theaters = theater;
                return Page();
            }

            // Cập nhật TheaterId cho user
            user.TheaterId = TheaterId;
            var success = await _userService.AssignTheaterAsync(user.Id, TheaterId);

            if (success)
            {
                TempData["Message"] = "Phân công rạp phim thành công!";
                return RedirectToPage("/Users/Index");
            }

            TempData["Message"] = "Không thể phân công rạp phim. Vui lòng thử lại.";
            User = user;
            Theaters = await _theatersService.ListAllTheaterAsync();
            return Page();
        }
    }
}
