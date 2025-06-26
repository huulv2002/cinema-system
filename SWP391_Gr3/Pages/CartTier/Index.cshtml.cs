using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.CartTier
{
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;

        public CreateModel(Swp391Context context)
        {
            _context = context;
        }

        public string UserFullName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string UserPhone { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Users/Login");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToPage("/Users/Login");

            UserFullName = user.FullName;
            UserEmail = user.Email;
            UserPhone = user.PhoneNumber;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out int userId))
                return RedirectToPage("/Users/Login");

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return RedirectToPage("/Users/Login");

            // Kh�ng cho t?o l?i n?u ?� c� th?
            var existed = await _context.LoyaltyCards.AnyAsync(c => c.UserId == user.Id);
            if (existed)
            {
                TempData["Error"] = "B?n ?� c� th? th�nh vi�n.";
                return RedirectToPage("/LoyaltyCards/Index");
            }

            // L?y c?p ?? Basic ho?c MinSpending = 0
            var rawTiers = await _context.CardTiers
     .Where(t => t.TierName != null)
     .Select(t => new {
         t.TierId,
         t.TierName,
         t.MinSpending
     })
     .ToListAsync();

            var tier = rawTiers.FirstOrDefault(t =>
                t.TierName.ToLower() == "member" || t.MinSpending == 0);

            if (tier == null)
            {
                ModelState.AddModelError("", "Kh�ng t�m th?y c?p ?? m?c ??nh.");
                return Page();
            }


            if (tier == null)
            {
                ModelState.AddModelError("", "Kh�ng t�m th?y c?p ?? Basic.");
                return Page();
            }

            // T?o m?i LoyaltyCard
            var newCard = new LoyaltyCard
            {
                UserId = user.Id,
                CardNumber = Guid.NewGuid().ToString("N")[..8].ToUpper(),
                CardType = "Member",
                TierId = tier.TierId, // g�n t? anonymous object
                RegisterDate = DateTime.Now,
                Status = "Active"
            };


            _context.LoyaltyCards.Add(newCard);
            await _context.SaveChangesAsync();

            TempData["Success"] = "T?o th? th�nh c�ng!";
            return RedirectToPage("/LoyaltyCards/Index");
        }
    }
}
