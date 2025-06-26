using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Promotions
{
    [AuthorizeRole("Owner")]
    public class PromotionTypeModel : PageModel
    {
        private readonly IPromotionService _promotionService;

        public PromotionTypeModel(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        public IEnumerable<PromotionType> PromotionTypes { get; set; } = new List<PromotionType>();

        public async Task OnGetAsync()
        {
            PromotionTypes = await _promotionService.ListAllPromotionTypeAsync();
        }
    }
}