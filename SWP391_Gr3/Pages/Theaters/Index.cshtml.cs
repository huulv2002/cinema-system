﻿using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> OnPostToggleActiveAsync(int Theaterid)
        {
            var success = await _theatersService.ToggleTheaterActiveStatusAsync(Theaterid);
            if (success)
            {
                TempData["Message"] = "Cập nhật trạng thái thành công!";
            }
            else
            {

                TempData["Message"] = "Không tìm thấy cập nhật thất bại.";

            }
            return RedirectToPage("./Index");

        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var result = await _theatersService.DeleteAsync(id);
            TempData["Message"] = result.Message;
            return RedirectToPage("./Index");
        }

    }
}
