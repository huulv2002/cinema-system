using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

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
            try
            {
                if (user.IsActive == false)
                {
                    ErrorMessage = "chưa active tài khoản";
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "chưa active tài khoản hoặc chưa có tài khoản";
                return Page();
            }
            if (user == null)
            {
                ErrorMessage = "Sai email hoặc mật khẩu";
                return Page();
            }
            var roleName = await _userSer.GetRoleNameAsync(user.Id);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, roleName ?? "")
            };

            // Tạo identity và principal
            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Đăng nhập bằng cookie
            await HttpContext.SignInAsync("Cookies", claimsPrincipal);

            // (Tùy chọn) Lưu vào session nếu bạn vẫn cần
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserRole", roleName);

            return RedirectToPage("/Home/Index");

        }
    }
}
