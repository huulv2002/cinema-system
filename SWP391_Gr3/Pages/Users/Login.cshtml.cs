using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class LoginModel : PageModel
    {
        private readonly IUserService _userSer;
        public LoginModel(IUserService userSer)
        {
            _userSer = userSer;
        }
        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userSer.ValidateUserAsync(Email, Password);
            if (user == null)
            {
                ErrorMessage = "Sai email hoặc mật khẩu";
                return Page();
            }
            var roleName = await _userSer.GetRoleNameAsync(user.Id);
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", roleName);

            return RedirectToPage("/Home/Index");
        }
    }
}
