using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Repositories
{
    public interface ITheatersRepository
    {
        Task<bool> ToggleTheaterActiveStatusAsync(int theaterId);
        Task<bool> AddTheaterAsync(Theater theater);
        Task<bool> AddRoomAsync(int id, Room room);
        Task<IEnumerable<Theater>> GetAllTheaterssAsync();
        Task<IEnumerable<Room>> GetAllRoomsAsync(int Thid);
        Task<bool> RoomCodeExistsAsync(string code, int theaterId);
        Task<OperationResult> DeleteTheaterAsync(int id);
    }
}
