using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;

namespace SWP391_Gr3.Services
{
    public class MoviesService : IMoviesService
    {
        private readonly IMoviesRepository _repository;

        public MoviesService(IMoviesRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Movie>> ListAllMoviesAsync()
        {
            return await _repository.ListAllMoviesAsync();
        }
    }
}
