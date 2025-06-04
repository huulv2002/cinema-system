using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System;

namespace SWP391_Gr3.Pages.Users
{
    public class ChangePasswordModel : PageModel
    {
        private readonly IUserService _userService;

        public ChangePasswordModel(IUserService userService)
        {
            _userService = userService;
        }

        [BindProperty, Required(ErrorMessage = "Bạn phải nhập mail")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu hiện tại")]
        public string CurrentPassword { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu mới")]
        [MinLength(6, ErrorMessage = "Mật khẩu mới phải có ít nhất 6 ký tự")]
        public string NewPassword { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "Bạn phải nhập mật khẩu mới")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhập không khớp")]
        public string ConfirmPassword { get; set; }

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }

        public void OnGet()
        {
            
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userService.GetUserByEmailAsync(Email);
            if (user == null)
            {
                ErrorMessage = "Email không tồn tại.";
                return Page();
            }

            var isValid = await _userService.ValidateUser(Email, CurrentPassword);
            if (!isValid)
            {
                ErrorMessage = "Mật khẩu hiện tại không đúng.";
                return Page();
            }

            var result = await _userService.UpdatePassword(Email, NewPassword);
            if (!result)
            {
                ErrorMessage = "Thất bại.";
                return Page();
            }

            SuccessMessage = "Thành công!";
            ModelState.Clear();
            return Page();
        }

    }
}
