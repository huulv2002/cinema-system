using SWP391_Gr3.Models;
using SWP391_Gr3.ViewModels;

namespace SWP391_Gr3.Services
{
    public interface ITheatersService
    {
        Task<bool> ToggleTheaterActiveStatusAsync(int theaterId);
        Task<bool> RoomCodeExistsAsync(string code, int theaterId);
        Task<bool> CreateTheaterAsync(TheaterViewModel theaterViewModel);
        Task<bool> CreateRoomAsync(Room room,int id);
        Task<IEnumerable<Theater>> ListAllTheaterAsync();
        Task<IEnumerable<Room>> ListAllRoomrAsync(int thid);
        Task<OperationResult> DeleteAsync(int id);
    }
}