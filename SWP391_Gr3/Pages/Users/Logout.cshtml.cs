using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SWP391_Gr3.Pages.Users
{
    public class LogoutModel : PageModel
    {
         public IActionResult OnGet() 
        { HttpContext.Session.Clear();
            return RedirectToPage("/Users/Login"); 
        }
    }    
}
