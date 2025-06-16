using Microsoft.EntityFrameworkCore;
using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public class MoviesRepository : IMoviesRepository
    {
        private readonly Swp391Context _context;

        public MoviesRepository(Swp391Context context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> ListAllMoviesAsync()
        {
            return await _context.Movies.Include(m => m.Genres)
                .Include(m => m.Images)
                .Include(m => m.Showtimes)
                .ToListAsync();
        }
    }
}
