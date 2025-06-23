using SWP391_Gr3.Models;

namespace SWP391_Gr3.Repositories
{
    public interface ITheatersRepository
    {
        Task<bool> AddTheaterAsync(Theater theater);
        Task<bool> AddRoomAsync(int id, Room room);
        Task<IEnumerable<Theater>> GetAllTheaterssAsync();
        Task<IEnumerable<Room>> GetAllRoomsAsync(int Thid);
        Task<bool> RoomCodeExistsAsync(string code, int theaterId);

    }
}
