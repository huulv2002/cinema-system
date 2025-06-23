using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Theaters
{
    [AuthorizeRole("Owner")]
    public class IndexModel : PageModel
    {
        private readonly ITheatersService _theatersService;

        public IndexModel(ITheatersService theatersService)
        {
            _theatersService = theatersService;
        }
        public IEnumerable<Theater> Theaters { get; set; } = new List<Theater>();
        public async Task OnGetAsync()
        {
            Theaters = await _theatersService.ListAllTheaterAsync();
        }

    }
}
