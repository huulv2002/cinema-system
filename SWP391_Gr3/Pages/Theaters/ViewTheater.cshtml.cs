using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Pages.Theaters
{
    public class ViewTheaterModel : PageModel
    {
        private readonly Swp391Context _context;

        public ViewTheaterModel(Swp391Context context)
        {
            _context = context;
        }

        public List<Theater> Theaters { get; set; } = new();
        public Dictionary<string, List<Theater>> CityTheaterMap { get; set; } = new();
        public Dictionary<int, List<IGrouping<Movie, Showtime>>> ShowtimesByTheater { get; set; } = new();
        public List<string> Locations { get; set; } = new();
        public string? UserRole { get; set; }

        public async Task OnGetAsync()
        {
            UserRole = HttpContext.Session.GetString("UserRole");

            if (UserRole == "Admin" || UserRole == "Owner")
            {
                Theaters = await _context.Theaters
                    .Include(t => t.Rooms)
                        .ThenInclude(r => r.Showtimes)
                            .ThenInclude(s => s.Movie)
                    .Include(t => t.Rooms)
                        .ThenInclude(r => r.Showtimes)
                            .ThenInclude(s => s.Movie.Images)
                    .ToListAsync();
            }
            else
            {
                Theaters = await _context.Theaters
                    .Where(t => t.IsActive == true)
                    .Include(t => t.Rooms)
                        .ThenInclude(r => r.Showtimes)
                            .ThenInclude(s => s.Movie)
                    .Include(t => t.Rooms)
                        .ThenInclude(r => r.Showtimes)
                            .ThenInclude(s => s.Movie.Images)
                    .ToListAsync();
            }

            CityTheaterMap = Theaters
                .Where(t => !string.IsNullOrEmpty(t.Location))
                .Select(t => new
                {
                    Theater = t,
                    City = ExtractCityFromLocation(t.Location!)
                })
                .GroupBy(x => x.City)
                .ToDictionary(g => g.Key, g => g.Select(x => x.Theater).ToList());

            Locations = CityTheaterMap.Keys.OrderBy(c => c).ToList();

            // Group showtimes by TheaterId => Movie => List<Showtime>
            foreach (var theater in Theaters)
            {
                var showtimes = theater.Rooms
                    .SelectMany(r => r.Showtimes)
                    .Where(s => s.StartTime != null && s.Movie != null)
                    .GroupBy(s => s.Movie!)
                    .ToList();

                ShowtimesByTheater[theater.Id] = showtimes;
            }
        }

        private string ExtractCityFromLocation(string location)
        {
            if (string.IsNullOrEmpty(location)) return "Khác";
            int lastCommaIndex = location.LastIndexOf(',');
            if (lastCommaIndex >= 0 && lastCommaIndex < location.Length - 1)
            {
                return location.Substring(lastCommaIndex + 1).Trim();
            }
            return location.Trim();
        }
    }
}
