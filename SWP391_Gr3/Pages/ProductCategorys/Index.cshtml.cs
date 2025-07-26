using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ProductCategorys
{
    [AuthorizeRole("Owner, Staff")]
    public class IndexModel : PageModel
    {
        private readonly Swp391Context _context;

        public IndexModel(Swp391Context context)
        {
            _context = context;
        }

        public List<ProductCategory> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            Categories = await _context.ProductCategories
                                       .Where(c => c.IsActive)
                                       .ToListAsync();
        }
    }

}
