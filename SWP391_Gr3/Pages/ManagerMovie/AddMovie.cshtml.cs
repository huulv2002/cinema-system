using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerMovie
{
    [AuthorizeRole("Owner")]

    public class AddMovieModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly IWebHostEnvironment _environment;

        public AddMovieModel(Swp391Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        [BindProperty]
        public IFormFile? UploadedImage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || UploadedImage == null)
            {
                return Page();
            }

            // Lưu phim trước
            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            // Tạo đường dẫn lưu ảnh
            var uploadFolder = Path.Combine(_environment.WebRootPath, "images", "movies");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(UploadedImage.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await UploadedImage.CopyToAsync(stream);
            }

            // Lưu URL ảnh vào bảng Image
            var image = new Image
            {
                Url = $"/images/movies/{fileName}",
                MovieId = Movie.Id
            };

            _context.Images.Add(image);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
