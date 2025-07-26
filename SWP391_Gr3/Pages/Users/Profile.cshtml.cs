using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Dtos;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    [AuthorizeRole("Customer")]
    public class ProfileModel : PageModel
    {
        private readonly IUserService _userService;

        public ProfileModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty]
        public UserProfileDto ProfileDto { get; set; }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                int userId = int.Parse(HttpContext.Session.GetString("UserId"));
                var user = await _userService.GetUserById(userId);

                if (user == null)
                {
                    ErrorMessage = "Không tìm thấy người dùng.";
                    return Page();
                }

                ProfileDto = new UserProfileDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Address = user.Address
                };

                return Page();
            }
            catch
            {
                ErrorMessage = "Lỗi khi tải thông tin người dùng.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Dữ liệu không hợp lệ.";
                return Page();
            }

            var result = await _userService.UpdateProfileAsync(ProfileDto);

            SuccessMessage = result ? "Cập nhật thành công!" : "Cập nhật thất bại.";
            return Page();
        }
    }
}
