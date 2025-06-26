using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
using SWP391_Gr3.Services;
using SWP391_Gr3.ViewModels;
using System.Threading.Tasks;

namespace SWP391_Gr3.Pages.Promotions
{
    [AuthorizeRole("Owner,Staff")]
    public class PromotionUpdateModel : PageModel
    {
        private readonly IPromotionService _promotionService;
        private readonly IWebHostEnvironment _environment;

        public PromotionUpdateModel(IPromotionService promotionService, IWebHostEnvironment environment)
        {
            _promotionService = promotionService;
            _environment = environment;
        }

        [BindProperty]
        public PromotionViewModel Promotion { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        public IEnumerable<PromotionType> PromotionTypes { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var promotion = await _promotionService.ListAllPromotionAsync();
            var selectedPromotion = promotion.FirstOrDefault(p => p.Id == id);
            if (selectedPromotion == null)
            {
                TempData["Message"] = "Không tìm thấy khuyến mãi.";
                return RedirectToPage("/Promotions/Manage");
            }

            Promotion = new PromotionViewModel
            {
                Id = selectedPromotion.Id,
                Code = selectedPromotion.Code,
                PromotionTypeId = selectedPromotion.PromotionTypeId ?? 0,
                Value = selectedPromotion.Value,
                Stock = selectedPromotion.Stock,
                StartDate = selectedPromotion.StartDate,
                EndDate = selectedPromotion.EndDate,
                ImageUrl = selectedPromotion.ImageUrl,
                Description = selectedPromotion.Description,
                IsActive = selectedPromotion.IsActive
            };

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

            // Set IsActive based on user role
            var userRole = HttpContext.Session.GetString("UserRole");
            Promotion.IsActive = userRole == "Owner";

            // Image validation and upload
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
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(fileStream);
                }

                Promotion.ImageUrl = $"/images/promotions/{uniqueFileName}";
            }

            var result = await _promotionService.UpdatePromotionAsync(Promotion);
            if (result)
            {
                TempData["Message"] = "Khuyến mãi đã được cập nhật thành công.";
                return RedirectToPage("/Promotions/Manage");
            }

            ModelState.AddModelError("", "Không thể cập nhật khuyến mãi.");
            PromotionTypes = await _promotionService.ListAllPromotionTypeAsync();
            return Page();
        }
    }
}