using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
<<<<<<< HEAD
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;
=======
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

>>>>>>> 99deb1117390132e47e998f37cb19f9d395f2a9b
namespace SWP391_Gr3.Pages.Reviews
{
    [AuthorizeRole("Owner,Staff")]
    public class CreateModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly ILogger<CreateModel> _logger;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(Swp391Context context, ILogger<CreateModel> logger, IWebHostEnvironment environment)
        {
            _context = context;
            _logger = logger;
            _environment = environment;
        }

        [BindProperty]
        public MovieReview MovieReview { get; set; } = new MovieReview();

        public SelectList MovieSelectList { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            MovieSelectList = new SelectList(movies, "Id", "Title");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var movies = await _context.Movies.ToListAsync();
            MovieSelectList = new SelectList(movies, "Id", "Title");

            // Kiểm tra khoảng trắng trống
            if (string.IsNullOrWhiteSpace(MovieReview.Title))
            {
                ModelState.AddModelError("MovieReview.Title", "Tiêu đề không được để trống hoặc chỉ chứa khoảng trắng.");
            }
            if (string.IsNullOrWhiteSpace(MovieReview.Summary))
            {
                ModelState.AddModelError("MovieReview.Summary", "Tóm tắt không được để trống hoặc chỉ chứa khoảng trắng.");
            }
            if (string.IsNullOrWhiteSpace(MovieReview.Content))
            {
                ModelState.AddModelError("MovieReview.Content", "Nội dung không được để trống hoặc chỉ chứa khoảng trắng.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Trim nội dung để lưu gọn gàng
            MovieReview.Title = MovieReview.Title.Trim();
            MovieReview.Summary = MovieReview.Summary?.Trim();
            MovieReview.Content = MovieReview.Content.Trim();

            // Kiểm tra ảnh
            if (ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "Vui lòng chọn một ảnh.");
                return Page();
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("ImageFile", "Chỉ chấp nhận ảnh có đuôi: .jpg, .jpeg, .png, .gif");
                return Page();
            }

            if (ImageFile.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("ImageFile", "Ảnh quá lớn. Vui lòng chọn ảnh nhỏ hơn 2MB.");
                return Page();
            }

            var fileName = Guid.NewGuid().ToString() + extension;
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
            Directory.CreateDirectory(uploadsFolder);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await ImageFile.CopyToAsync(stream);
            }

            MovieReview.ImageUrl = "/images/" + fileName;
            MovieReview.PublishedDate = DateTime.UtcNow;
            MovieReview.Likes = 0;
            MovieReview.Views = 0;

            _context.MovieReviews.Add(MovieReview);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
