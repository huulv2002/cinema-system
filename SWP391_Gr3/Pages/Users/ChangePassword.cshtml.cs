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

        [BindProperty, Required(ErrorMessage = "B?n ph?i nh?p email")]
        [EmailAddress(ErrorMessage = "Email kh�ng h?p l?")]
        public string Email { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "B?n ph?i nh?p m?t kh?u hi?n t?i")]
        public string CurrentPassword { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "B?n ph?i nh?p m?t kh?u m?i")]
        [MinLength(6, ErrorMessage = "M?t kh?u m?i ph?i c� �t nh?t 6 k? t?")]
        public string NewPassword { get; set; }

        [BindProperty, DataType(DataType.Password)]
        [Required(ErrorMessage = "B?n ph?i x�c nh?n m?t kh?u m?i")]
        [Compare("NewPassword", ErrorMessage = "M?t kh?u x�c nh?n kh�ng kh?p")]
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
                ErrorMessage = "Email kh�ng t?n t?i.";
                return Page();
            }

            var isValid = await _userService.ValidateUser(Email, CurrentPassword);
            if (!isValid)
            {
                ErrorMessage = "M?t kh?u hi?n t?i kh�ng ��ng.";
                return Page();
            }

            var result = await _userService.UpdatePassword(Email, NewPassword);
            if (!result)
            {
                ErrorMessage = "�?i m?t kh?u th?t b?i.";
                return Page();
            }

            SuccessMessage = "�?i m?t kh?u th�nh c�ng!";
            ModelState.Clear();
            return Page();
        }

    }
}
