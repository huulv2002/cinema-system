using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public interface IMoviesRepository
    {
        Task<IEnumerable<Movie>> ListAllMoviesAsync();
    }
}
