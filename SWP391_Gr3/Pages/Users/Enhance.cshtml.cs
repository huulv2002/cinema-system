using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Users
{
    public class EnhanceModel : PageModel
    {

        private readonly IUserService _userSer;
        private readonly IRoleSer _roleSer;

        public EnhanceModel(IUserService userSer, IRoleSer roleSer)
        {
            _userSer = userSer;
            _roleSer = roleSer;
        }

        [BindProperty]

        public List<Role> Roles { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            Roles = (await _roleSer.GetAllRoleAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {



            return RedirectToPage("/Users/Index");
        }
    }
}
