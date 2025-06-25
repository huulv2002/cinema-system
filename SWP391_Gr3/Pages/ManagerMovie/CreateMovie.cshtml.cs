using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SWP391_Gr3.Autho;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.ManagerMovie
{
    [AuthorizeRole("Owner")]
    public class CreateMovieModel : PageModel
    {
        private readonly Swp391Context _context;
        private readonly IWebHostEnvironment _environment;

        public CreateMovieModel(Swp391Context context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty] public Movie Movie { get; set; }
        [BindProperty] public IFormFile? UploadedImage { get; set; }
        [BindProperty] public int DurationHours { get; set; }
        [BindProperty] public int? DurationMinutes { get; set; }

        public string? DurationErrorMessage { get; set; }
        public string? ImageErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            // 1. XỬ LÝ THỜI LƯỢNG
            int mins = DurationMinutes ?? 0;
            if (DurationHours < 0 || DurationHours > 10 || mins < 0 || mins > 59)
            {
                DurationErrorMessage = "Giờ phải 0–10, phút 0–59.";
            }
            int totalMinutes = DurationHours * 60 + mins;
            if (totalMinutes <= 0)
            {
                DurationErrorMessage = "Thời lượng phải lớn hơn 0.";
            }

            // 2. Validate các trường khác
            if (string.IsNullOrWhiteSpace(Movie.Title) || Movie.Title.Length > 100)
                ModelState.AddModelError("Movie.Title", "Tiêu đề không được để trống và tối đa 100 ký tự.");

            if (string.IsNullOrWhiteSpace(Movie.Description) || Movie.Description.Length > 10000)
                ModelState.AddModelError("Movie.Description", "Mô tả không được để trống và tối đa 10000 ký tự.");

            if (Movie.OverAge < 1 || Movie.OverAge > 99)
                ModelState.AddModelError("Movie.OverAge", "Tuổi giới hạn phải từ 1 đến 99.");

            if (string.IsNullOrWhiteSpace(Movie.Director) || Movie.Director.Length > 100)
                ModelState.AddModelError("Movie.Director", "Đạo diễn không được để trống và tối đa 100 ký tự.");

            if (string.IsNullOrWhiteSpace(Movie.Performer) || Movie.Performer.Length > 1000)
                ModelState.AddModelError("Movie.Performer", "Diễn viên không được để trống và tối đa 1000 ký tự.");

            if (string.IsNullOrWhiteSpace(Movie.Language) || Movie.Language.Length > 100)
                ModelState.AddModelError("Movie.Language", "Ngôn ngữ không được để trống và tối đa 100 ký tự.");

            if (Movie.ReleaseDate == default)
                ModelState.AddModelError("Movie.ReleaseDate", "Ngày phát hành không hợp lệ.");

            // 3. Validate ảnh
            if (UploadedImage == null || UploadedImage.Length == 0)
            {
                ImageErrorMessage = "Vui lòng chọn ảnh phim.";
            }

            // 4. Nếu có lỗi validation, trả lại form
            if (!ModelState.IsValid || UploadedImage == null || totalMinutes <= 0)
            {
                return Page();
            }

            // 5. Gán và lưu Movie
            Movie.Duration = totalMinutes;
            _context.Movies.Add(Movie);
            await _context.SaveChangesAsync();

            // 6. Lưu ảnh vào /wwwroot/images/movies
            var uploadFolder = Path.Combine(_environment.WebRootPath, "images", "movies");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            var fileName = Guid.NewGuid() + Path.GetExtension(UploadedImage.FileName);
            var filePath = Path.Combine(uploadFolder, fileName);
            using var stream = new FileStream(filePath, FileMode.Create);
            await UploadedImage.CopyToAsync(stream);

            _context.Images.Add(new Image
            {
                Url = "/images/movies/" + fileName,
                MovieId = Movie.Id
            });
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
