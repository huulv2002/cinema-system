using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;

namespace SWP391_Gr3.Pages.Promotions
{
    [AuthorizeRole("Owner,Staff")]
    public class ManageModel : PageModel
    {
        private readonly IPromotionService _promotionService;

        public ManageModel(IPromotionService promotionService)
        {
            _promotionService = promotionService;           
        }

        public IList<Promotion> Promotions { get; set; }
        public string SearchUserId { get; private set; }
        public List<Promotion> FilteredPro { get; private set; }
        public async Task OnGetAsync(string proId)
        {
            var promotions = await _promotionService.ListAllPromotionAsync();
            SearchUserId = proId;
            FilteredPro = string.IsNullOrEmpty(proId)
                ? promotions.ToList()
                : promotions.Where(u => u.Id.ToString() == proId).ToList();
        }
    }
}
