using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Reviews
{
    [AuthorizeRole("Owner,Staff")]
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
        public MovieReview MovieReview { get; set; } = default!;

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
                return NotFound();

            var moviereview = await _context.MovieReviews.FindAsync(id);
            if (moviereview == null)
                return NotFound();

            MovieReview = moviereview;
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", MovieReview.MovieId);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Title", MovieReview.MovieId);

            // Custom validation
            if (string.IsNullOrWhiteSpace(MovieReview.Title))
                ModelState.AddModelError("MovieReview.Title", "Title không được để trống hoặc chỉ chứa dấu cách.");
            if (string.IsNullOrWhiteSpace(MovieReview.Summary))
                ModelState.AddModelError("MovieReview.Summary", "Summary không được để trống hoặc chỉ chứa dấu cách.");
            if (string.IsNullOrWhiteSpace(MovieReview.Content))
                ModelState.AddModelError("MovieReview.Content", "Content không được để trống hoặc chỉ chứa dấu cách.");

            if (ImageFile != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(ImageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                    ModelState.AddModelError("ImageFile", "Chỉ chấp nhận ảnh đuôi .jpg, .jpeg, .png, .gif");

                if (ImageFile.Length > 2 * 1024 * 1024)
                    ModelState.AddModelError("ImageFile", "Ảnh quá lớn. Vui lòng chọn ảnh nhỏ hơn 2MB.");
            }

            if (!ModelState.IsValid)
                return Page();

            var reviewInDb = await _context.MovieReviews.FindAsync(MovieReview.Id);
            if (reviewInDb == null)
                return NotFound();

            // Update fields
            reviewInDb.Title = MovieReview.Title.Trim();
            reviewInDb.Summary = MovieReview.Summary?.Trim();
            reviewInDb.Content = MovieReview.Content.Trim();
            reviewInDb.PublishedDate = MovieReview.PublishedDate;
            reviewInDb.MovieId = MovieReview.MovieId;

            if (ImageFile != null)
            {
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                Directory.CreateDirectory(uploadsFolder);
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                reviewInDb.ImageUrl = "/uploads/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
