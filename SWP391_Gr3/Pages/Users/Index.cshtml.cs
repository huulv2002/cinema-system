using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    [AuthorizeRole("Admin,Owner")]
    public class IndexModel : PageModel
    {
        private readonly IUserService _iusersSer;
        public IndexModel(IUserService iusersSer)
        {
            _iusersSer = iusersSer;
        }
        public List<User> FilteredUsers { get; set; }
        public string SearchUserId { get; set; }
        public async Task OnGetAsync(string userId)
        {
            var users = await _iusersSer.GetAllUsersAsync();
            SearchUserId = userId;
            FilteredUsers = string.IsNullOrEmpty(userId)
                ? users.ToList()
                : users.Where(u => u.Id.ToString() == userId).ToList();
        }

        public async Task<IActionResult> OnPostToggleActiveAsync(int id)
        {

            int userId = int.Parse(HttpContext.Session.GetString("UserId"));
            var role = HttpContext.Session.GetString("UserRole");
            var users = await _iusersSer.GetUserById(id);
            if (users == null)
            {
                return NotFound();
            }
            if (userId == id)
            {
                TempData["Message"] = "tài khoản đang đăng nhập không thể tác động";
                return RedirectToPage("./Index");
            }
            if (role == "Owner")
            {
                var success1 = await _iusersSer.ToggleUserActiveStatusAsync(id);
                if (success1)
                {
                    TempData["Message"] = "Cập nhật trạng thái tài khoản thành công!";
                }
                else
                {
                    TempData["Message"] = "Không tìm thấy tài khoản hoặc cập nhật thất bại.";
                }
                return RedirectToPage("./Index");
            }
            else if (role == "Admin")
            {
                if (users.RoleId == 3 || users.RoleId == 1)
                {
                    TempData["Message"] = "Không đủ thẩm quyền";
                    return RedirectToPage("./Index");
                }
            }
            var success = await _iusersSer.ToggleUserActiveStatusAsync(id);

            if (success)
            {
                TempData["Message"] = "Cập nhật trạng thái tài khoản thành công!";
            }
            else
            {
                TempData["Message"] = "Không tìm thấy tài khoản hoặc cập nhật thất bại.";
            }
            return RedirectToPage("./Index");
        }

    }
}
