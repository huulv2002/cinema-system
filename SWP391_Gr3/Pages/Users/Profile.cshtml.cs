using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;

        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public User currentUser { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int userId;
            try
            {
                userId = int.Parse(HttpContext.Session.GetString("UserId"));
                currentUser = await _userService.GetUserById(userId);
            }
            catch (Exception e)
            {
                ErrorMessage = "Không tìm thấy người dùng";
                return Page();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Dữ liệu không hợp lệ";
                return Page();
            }

            var result = await _userService.UpdateProfile(currentUser);
            if (result)
            {
                SuccessMessage = "Cập nhập thông tin thành công!";
            }
            else
            {
                ErrorMessage = "Cập nhập thất bại.";
            }

            return Page();
        }
    }
}