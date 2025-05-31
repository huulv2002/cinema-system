using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Theaters
{
    public class ViewTheaterModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewTheaterModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Theater> Theaters { get; set; } = new();
        public string? UserRole { get; set; }

        public async Task OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");

            if (UserRole == "Admin" || UserRole == "Owner")
            {
                Theaters = await _context.Theaters.ToListAsync();
            }
            else
            {
                Theaters = await _context.Theaters
                    .Where(t => t.IsActive == true)
                    .ToListAsync();
            }
        }
    }
}
