using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Users
{
    public class RegisterModel : PageModel
    {
        private readonly IUserService _userSer;
        public RegisterModel(IUserService userSer)
        {
            _userSer = userSer;
        }
        [BindProperty]
        public RegisterUser RegisterUser { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string confirmPasswordHash)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (RegisterUser.HashPass != confirmPasswordHash)
            {
                ErrorMessage = "Mật khẩu không khớp!";
                return Page();
            }

            var user = new User
            {
                FullName = RegisterUser.FullName,
                Email = RegisterUser.Email,
                HashPass = RegisterUser.HashPass,
                PhoneNumber = RegisterUser.PhoneNumber,
                Address = RegisterUser.address,
                RoleId = RegisterUser.role,
                IsActive = RegisterUser.isActive
            };

            bool success = await _userSer.RegisterUserAsync(user);
            if (!success)
            {
                ErrorMessage = "Đăng ký thất bại kiểm tra lại email có thể bị trùng";
                return Page();
            }

            return RedirectToPage("./Login");
        }
    }
}
