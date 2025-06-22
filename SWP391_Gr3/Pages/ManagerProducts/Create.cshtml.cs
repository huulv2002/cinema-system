using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProducts
{
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly IWebHostEnvironment _environment;
        public CreateModel(Swp391Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Product Product { get; set; } = new();
        [BindProperty]
        public IFormFile? ImageFile { get; set; }
        public List<ProductCategory> Categories { get; set; } = new();

        public void OnGet()
        {
            Categories = _context.ProductCategories.ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Categories = _context.ProductCategories.ToList();
                return Page();
            }

        
            if (ImageFile != null && ImageFile.Length > 0)
            {
              
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".jfif" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Chỉ được chọn file ảnh (.jpg, .jpeg, .png, .webp, .jfif )");
                    Categories = _context.ProductCategories.ToList();
                    return Page();
                }

                var fileName = Guid.NewGuid().ToString() + extension;
                var savePath = Path.Combine(_environment.WebRootPath, "images", "product");
                if (!Directory.Exists(savePath))
                    Directory.CreateDirectory(savePath);
                var filePath = Path.Combine(savePath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                Product.ImageUrl = "/images/product/" + fileName;
            }
          
            Product.IsActive = true;
            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }

    }
}
