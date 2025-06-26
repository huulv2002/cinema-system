using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Promotions
{
    [AuthorizeRole("Owner")]
    public class CreatePromotionTypeModel : PageModel
    {
        private readonly IPromotionService _promotionService;

        public CreatePromotionTypeModel(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [BindProperty]
        public PromotionType PromotionType { get; set; } = new PromotionType();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _promotionService.CreatePromotionTypeAsync(PromotionType);
            if (result)
            {
                return RedirectToPage("/Promotions/PromotionType");
            }

            ModelState.AddModelError("", "Không thể tạo loại khuyến mại. Vui lòng thử lại.");
            return Page();
        }
    }
}