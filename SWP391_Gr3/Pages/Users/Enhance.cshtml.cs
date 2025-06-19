using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class EnhanceModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IRoleRepo _roleRepo;

        public EnhanceModel(IUserService userService, IRoleRepo roleRepo)
        {
            _userService = userService;
            _roleRepo = roleRepo;
        }

        [BindProperty]
        public List<Role> Roles { get; set; }

        [BindProperty]
        public int SelectedRoleId { get; set; }

        [BindProperty]
        public int UserId { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            UserId = id;
            Roles = (await _roleRepo.GetAllRoleAsync()).ToList();
            if (Roles == null || !Roles.Any())
            {
                ErrorMessage = "Không tìm thấy vai trò nào.";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Roles = (await _roleRepo.GetAllRoleAsync()).ToList();
                ErrorMessage = "Vui lòng chọn một vai trò hợp lệ.";
                return Page();
            }

            var user = await _userService.GetUserByIdAsync(UserId);
            if (user == null)
            {
                ErrorMessage = "Không tìm thấy người dùng.";
                Roles = (await _roleRepo.GetAllRoleAsync()).ToList();
                return Page();
            }


            var selectedRole = await _roleRepo.GetAllRoleAsync();
            if (selectedRole.FirstOrDefault(r => r.Id == SelectedRoleId)?.Name == "Owner")
            {
                ErrorMessage = "Không thể nâng quyền lên vai trò Owner.";
                Roles = (await _roleRepo.GetAllRoleAsync()).ToList();
                return Page();
            }


            var result = await _userService.UpdateUserRoleAsync(UserId, SelectedRoleId);
            if (!result)
            {
                ErrorMessage = "Đã xảy ra lỗi khi cập nhật vai trò.";
                Roles = (await _roleRepo.GetAllRoleAsync()).ToList();
                return Page();
            }

            return RedirectToPage("/Users/UserDetail", new { id = UserId });
        }
    }
}