using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProductCombo
{
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Combo> Combos { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        [BindProperty(SupportsGet = true)]
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }

        public async Task OnGetAsync()
        {
            var query = _context.Combos
                .Where(c => c.IsActive == true)
                .Include(c => c.ProductCombos)
                    .ThenInclude(pc => pc.Product)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                query = query.Where(c => c.Title.Contains(SearchTerm));
            }

            int totalItems = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            Combos = await query
                .OrderByDescending(c => c.Id)
                .Skip((PageNumber - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();
        }
    }
}
