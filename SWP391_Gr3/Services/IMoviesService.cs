using SWP391_Gr3.Models;

namespace SWP391_Gr3.Services
{
    public interface IMoviesService
    {
        Task<IEnumerable<Movie>> ListAllMoviesAsync();
    }
}
