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

        [BindProperty] public Movie Movie { get; set; }

        [BindProperty] public IFormFile? ImageFile { get; set; }

        [BindProperty] public int DurationHours { get; set; }
        [BindProperty] public int DurationMinutes { get; set; }

        public List<Image> ExistingImages { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Movie = await _context.Movies.FindAsync(id);
            if (Movie == null) return NotFound();

            ExistingImages = await _context.Images.Where(i => i.MovieId == id).ToListAsync();

            // Phân tách thời lượng ra giờ và phút
            DurationHours = (int)(Movie.Duration / 60);
            DurationMinutes = (int)(Movie.Duration % 60);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validate thời lượng
            int totalDuration = (DurationHours * 60) + DurationMinutes;
            if (DurationHours < 0 || DurationHours > 10)
                ModelState.AddModelError("DurationHours", "Giờ phải từ 0 đến 10.");
            if (DurationMinutes < 0 || DurationMinutes >= 60)
                ModelState.AddModelError("DurationMinutes", "Phút phải từ 0 đến 59.");
            if (totalDuration <= 0)
                ModelState.AddModelError("Movie.Duration", "Thời lượng phải lớn hơn 0 phút.");

            // Validate các trường khác
            if (string.IsNullOrWhiteSpace(Movie.Title) || Movie.Title.Length > 1000)
                ModelState.AddModelError("Movie.Title", "Tiêu đề không được trống và không vượt quá 1000 ký tự.");

            if (string.IsNullOrWhiteSpace(Movie.Description) || Movie.Description.Length > 10000)
                ModelState.AddModelError("Movie.Description", "Mô tả không được trống và không vượt quá 10000 ký tự.");

            if (Movie.OverAge < 1 || Movie.OverAge > 99)
                ModelState.AddModelError("Movie.OverAge", "Tuổi giới hạn phải từ 1 đến 99.");

            if (string.IsNullOrWhiteSpace(Movie.Director) || Movie.Director.Length > 100)
                ModelState.AddModelError("Movie.Director", "Đạo diễn không được trống và không vượt quá 100 ký tự.");

            if (string.IsNullOrWhiteSpace(Movie.Performer) || Movie.Performer.Length > 1000)
                ModelState.AddModelError("Movie.Performer", "Diễn viên không được trống và không vượt quá 1000 ký tự.");

            if (string.IsNullOrWhiteSpace(Movie.Language) || Movie.Language.Length > 100)
                ModelState.AddModelError("Movie.Language", "Ngôn ngữ không được trống và không vượt quá 100 ký tự.");

            if (Movie.ReleaseDate == default)
                ModelState.AddModelError("Movie.ReleaseDate", "Ngày phát hành không hợp lệ.");

            if (!ModelState.IsValid)
            {
                ExistingImages = await _context.Images.Where(i => i.MovieId == Movie.Id).ToListAsync();
                return Page();
            }

            var movieInDb = await _context.Movies
                .Include(m => m.Images)
                .FirstOrDefaultAsync(m => m.Id == Movie.Id);

            if (movieInDb == null)
                return NotFound();

            // Cập nhật dữ liệu
            movieInDb.Title = Movie.Title;
            movieInDb.Description = Movie.Description;
            movieInDb.OverAge = Movie.OverAge;
            movieInDb.Director = Movie.Director;
            movieInDb.Performer = Movie.Performer;
            movieInDb.Duration = totalDuration;
            movieInDb.Language = Movie.Language;
            movieInDb.ReleaseDate = Movie.ReleaseDate;

            // Xử lý ảnh mới
            if (ImageFile != null && ImageFile.Length > 0)
            {
                if (movieInDb.Images.Any())
                    _context.Images.RemoveRange(movieInDb.Images);

                var fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                var folder = Path.Combine(_env.WebRootPath, "images", "movies");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fullPath = Path.Combine(folder, fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                _context.Images.Add(new Image
                {
                    MovieId = movieInDb.Id,
                    Url = "/images/movies/" + fileName
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("/ManagerMovie/Index");
        }
    }
}
