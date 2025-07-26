using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerProducts
{
    public class EditModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(Swp391Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Product Product { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<ProductCategory> Categories { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Product = await _context.Products.FindAsync(id);

            if (Product == null)
                return NotFound();

            Categories = _context.ProductCategories.Where(pc => pc.IsActive).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Categories = _context.ProductCategories.Where(pc => pc.IsActive).ToList();

            if (!ModelState.IsValid)
                return Page();

            var existingProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == Product.Id);

            if (existingProduct == null)
                return NotFound();

           
            var isDuplicate = _context.Products
                .Any(p => p.Id != Product.Id && p.Name == Product.Name && p.IsActive);

            if (isDuplicate)
            {
                ModelState.AddModelError("Product.Name", "Tên sản phẩm đã tồn tại.");
                return Page();
            }

         
            existingProduct.Name = Product.Name;
            existingProduct.Size = Product.Size;
            existingProduct.Price = Product.Price;
            existingProduct.Stock = Product.Stock;
            existingProduct.Description = Product.Description;
            existingProduct.ProductCategoryId = Product.ProductCategoryId;
            existingProduct.IsActive = true;

            
            if (ImageFile != null && ImageFile.Length > 0)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp", ".jfif" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ImageFile", "Chỉ được chọn file ảnh (.jpg, .jpeg, .png, .webp, .jfif)");
                    return Page();
                }

                var fileName = Guid.NewGuid() + extension;
                var folderPath = Path.Combine(_environment.WebRootPath, "images", "product");
                Directory.CreateDirectory(folderPath);
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                existingProduct.ImageUrl = "/images/product/" + fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("Index");
        }
    }
    }
