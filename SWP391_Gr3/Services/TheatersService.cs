using SWP391_Gr3.Models;
using SWP391_Gr3.Repositories;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Services
{
    public class TheatersService : ITheatersService
    {
        private readonly ITheatersRepository _repo;

        public TheatersService(ITheatersRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> CreateRoomAsync(Room room, int id)
        {
           return await _repo.AddRoomAsync(id, room);
        }

        public async Task<bool> CreateTheaterAsync(TheaterViewModel theaterViewModel)
        {
            var ths = new Theater()
            {
                Id = theaterViewModel.Id,
                Name = theaterViewModel.Name,
                Location = theaterViewModel.Location,
                IsActive = theaterViewModel.IsActive,
            };
            return await _repo.AddTheaterAsync(ths);
        }

        public async Task<IEnumerable<Room>> ListAllRoomrAsync(int thid)
        {
           return await _repo.GetAllRoomsAsync(thid);
        }

        public async Task<IEnumerable<Theater>> ListAllTheaterAsync()
        {
            return await _repo.GetAllTheaterssAsync();
        }

        public async Task<bool> RoomCodeExistsAsync(string code, int theaterId)
        {
            return await _repo.RoomCodeExistsAsync(code, theaterId);
        }
    }
}
