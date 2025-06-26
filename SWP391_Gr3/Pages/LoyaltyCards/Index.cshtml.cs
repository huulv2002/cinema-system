using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.LoyaltyCards
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<LoyaltyCard> Cards { get; set; } = new List<LoyaltyCard>();
        public User CurrentUser { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                // Redirect về trang login nếu chưa đăng nhập
                return RedirectToPage("/Users/Login");
            }

            if (!int.TryParse(userIdString, out int userId))
            {
                ErrorMessage = "Phiên đăng nhập không hợp lệ";
                ViewData["ErrorMessage"] = ErrorMessage;
                return Page();
            }

            try
            {
                CurrentUser = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (CurrentUser == null)
                {
                    ErrorMessage = "Không tìm thấy thông tin người dùng";
                    ViewData["ErrorMessage"] = ErrorMessage;
                    return Page();
                }

                Cards = await _context.LoyaltyCards
                    .Include(c => c.Tier) 
                    .Where(c => c.UserId == userId) 
                    .OrderByDescending(c => c.RegisterDate) 
                    .AsNoTracking() 
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading loyalty cards for user {userId}: {ex.Message}");

                Cards = new List<LoyaltyCard>();

                ErrorMessage = "Có lỗi xảy ra khi tải dữ liệu thẻ thành viên.";
                ViewData["ErrorMessage"] = ErrorMessage;
            }

            return Page();
        }
    }
}