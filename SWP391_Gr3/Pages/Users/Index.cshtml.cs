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

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _iusersSer.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _iusersSer.DeleteUserAsync(id);
            return RedirectToPage();
        }
    }
}
