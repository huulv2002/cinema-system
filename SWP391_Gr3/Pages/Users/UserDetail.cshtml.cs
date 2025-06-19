using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    [AuthorizeRole("Admin,Owner")]
    public class UserDetailModel : PageModel
    {
        private readonly IUserService _userService;

        public UserDetailModel(IUserService userService)
        {
            _userService = userService;
        }
        public User SelectedUser { get; set; }
        public string? ErrorMessage { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                // Lấy thông tin người dùng dựa trên id được truyền qua URL
                SelectedUser = await _userService.GetUserByIdAsync(id);
                if (SelectedUser == null)
                {
                    ErrorMessage = "Không tìm thấy người dùng";
                }
            }
            catch (Exception)
            {
                ErrorMessage = "Đã xảy ra lỗi khi lấy thông tin người dùng";
            }

            return Page();
        }
    }
}
