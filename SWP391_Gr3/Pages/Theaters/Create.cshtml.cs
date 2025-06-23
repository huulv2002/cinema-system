using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Theaters
{
    public class CreateModel : PageModel
    {
        private readonly ITheatersService _theatersService;

        public CreateModel(ITheatersService theatersService)
        {
            _theatersService = theatersService;
        }
        [BindProperty]
        public TheaterViewModel TheaterViewModel { get; set; } = new TheaterViewModel();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _theatersService.CreateTheaterAsync(TheaterViewModel);
            if (result)
            {
                return RedirectToPage("Index");
            }

            ModelState.AddModelError(string.Empty, "Không thể tạo rạp. Vui lòng thử lại.");
            return Page();
        }
    }
}
