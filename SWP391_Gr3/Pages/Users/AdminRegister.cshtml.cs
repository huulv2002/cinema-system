using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class AdminRegisterModel : PageModel
    {
        private readonly IUserService _userSer; 
        private readonly IRoleSer _roleSer;

        public AdminRegisterModel(IUserService userSer, IRoleSer roleSer)
        {
            _userSer = userSer;
            _roleSer = roleSer;
        }

        [BindProperty]
        public ViewModels.AdminRegister AdminRegister { get; set; }

        public List<Role> Roles { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Roles = (await _roleSer.GetAllRoleAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string confirmHashPass)
        {
            if (!ModelState.IsValid)
            {
                Roles = (await _roleSer.GetAllRoleAsync()).ToList();
                return Page();
            }

            if (AdminRegister.HashPass != confirmHashPass)
            {
                ErrorMessage = "Mật khẩu không khớp!";
                Roles = (await _roleSer.GetAllRoleAsync()).ToList();
                return Page();
            }

            var user = new User
            {
                FullName = AdminRegister.FullName,
                Email = AdminRegister.Email,
                HashPass = AdminRegister.HashPass,
                PhoneNumber = AdminRegister.PhoneNumber,
                Address = AdminRegister.address,
                RoleId = AdminRegister.roleId,
                IsActive = AdminRegister.isActive
            };

            bool success = await _userSer.RegisterUserAsync(user);
            if (!success)
            {
                ErrorMessage = "Đăng ký thất bại";
                Roles = (await _roleSer.GetAllRoleAsync()).ToList();
                return Page();
            }

            return RedirectToPage("/Home/Index");
        }
    }
}
