using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Pages.Promotions
{
    [AuthorizeRole("Owner,Staff")]
    public class CreateModel : PageModel
    {
        private readonly IPromotionService _promotionService;
        private readonly IWebHostEnvironment _environment;
        public CreateModel(IPromotionService promotionService, IWebHostEnvironment environment)
        {
            _promotionService = promotionService;
            _environment = environment;
        }
        [BindProperty]
        public PromotionViewModel Promotion { get; set; }
        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<PromotionType> PromotionTypes { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {
            PromotionTypes = await _promotionService.ListAllPromotionTypeAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PromotionTypes = await _promotionService.ListAllPromotionTypeAsync();
                return Page();
            }

            // Handle image upload
            if (ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Chỉ chấp nhận các định dạng hình ảnh: jpg, jpeg, png, gif.");
                    PromotionTypes = await _promotionService.ListAllPromotionTypeAsync();
                    return Page();
                }

                var uploadsFolder = Path.Combine(_environment.WebRootPath, "images/promotions");
                Directory.CreateDirectory(uploadsFolder); // Create directory if it doesn't exist
                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                Promotion.ImageUrl = $"/images/promotions/{uniqueFileName}";
            }

            var result = await _promotionService.CreatePromotionAsync(Promotion);
            if (result)
            {
                TempData["Message"] = "Khuyến mãi đã được tạo thành công.";
                return RedirectToPage("/Promotions/Manage");
            }

            ModelState.AddModelError("", "Không thể tạo khuyến mãi.");
            PromotionTypes = await _promotionService.ListAllPromotionTypeAsync();
            return Page();
        }
    }
}
