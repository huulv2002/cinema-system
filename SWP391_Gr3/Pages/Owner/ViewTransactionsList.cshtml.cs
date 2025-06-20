using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Owner
{
    public class ViewTransactionsListModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewTransactionsListModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Payment> Payments { get; set; }

        public async Task OnGetAsync()
        {
            Payments = await _context.Payments
                .Include(p => p.Order)
                    .ThenInclude(o => o.User)
                .OrderByDescending(p => p.Id)
                .ToListAsync();
        }
    }
}
