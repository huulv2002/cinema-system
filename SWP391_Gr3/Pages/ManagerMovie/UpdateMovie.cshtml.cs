using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerMovie
{
    [AuthorizeRole("Owner")]

    public class UpdateMovieModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly IWebHostEnvironment _env;

        public UpdateMovieModel(Swp391Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [BindProperty]
        public Movie Movie { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public List<Image> ExistingImages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies.FindAsync(id);
            if (Movie == null) return NotFound();

            ExistingImages = await _context.Images.Where(i => i.MovieId == id).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var movieInDb = await _context.Movies
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.Id == Movie.Id);

            if (movieInDb == null)
                return NotFound();

            // Cập nhật thông tin cơ bản
            movieInDb.Title = Movie.Title;
            movieInDb.Description = Movie.Description;
            movieInDb.OverAge = Movie.OverAge;
            movieInDb.Director = Movie.Director;
            movieInDb.Performer = Movie.Performer;
            movieInDb.Duration = Movie.Duration;
            movieInDb.Language = Movie.Language;
            movieInDb.ReleaseDate = Movie.ReleaseDate;

            // Nếu người dùng chọn ảnh mới
            if (ImageFile != null && ImageFile.Length > 0)
            {
                // Xoá ảnh cũ
                if (movieInDb.Images.Any())
                {
                    _context.Images.RemoveRange(movieInDb.Images);
                }

                // Lưu ảnh mới
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageFile.FileName);
                var savePath = Path.Combine(_env.WebRootPath, "images", "movies");

                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                var fullPath = Path.Combine(savePath, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                var image = new Image
                {
                    MovieId = movieInDb.Id,
                    Url = "/images/movies/" + fileName
                };
                _context.Images.Add(image);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/ManagerMovie/Index");
        }
    }
}
